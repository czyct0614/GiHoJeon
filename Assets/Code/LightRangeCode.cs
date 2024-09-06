using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRangeCode : MonoBehaviour
{
    public bool turnOff;
    public bool reset;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Color originalColor;
    private List<Collider2D> detectedEnemies = new List<Collider2D>(); // 이미 들어온 적들을 저장하는 리스트
    private Dictionary<Collider2D, float> originalVisionRangeSizes = new Dictionary<Collider2D, float>();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        boxCollider = GetComponent<BoxCollider2D>();
        reset = false;    
    }





    void Update()
    {

        // turnOff가 true일 때 색상 변경
        if (turnOff)
        {

            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.3f); // 불투명한 회색

            // detectedEnemies 리스트 복사 후 순회
            foreach (var enemy in new List<Collider2D>(detectedEnemies))
            {
                ProcessEnemy(enemy); // 이미 감지된 적에 대해서도 처리
            }
            
        }
        else
        {

            spriteRenderer.color = originalColor;

            if (reset)
            {

                // detectedEnemies 리스트 복사 후 순회
                foreach (var enemy in new List<Collider2D>(detectedEnemies))
                {

                    ReturnEnemy(enemy); // 이미 감지된 적에 대해서도 처리

                }

                StartCoroutine(DisableColliderTemporarily(0.1f));

                reset = false;

            }

        }

    }





    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("NewEnemy"))
        {

            originalVisionRangeSizes[other] = other.transform.Find("VisionRange").localScale.x;

            // 이미 감지된 적이 아니면 리스트에 추가
            if (!detectedEnemies.Contains(other))
            {

                detectedEnemies.Add(other);

            }

            if (turnOff)
            {

                ProcessEnemy(other); // 즉시 처리

            }

        }

    }





    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("NewEnemy"))
        {

            ReturnEnemy(other);

        }

    }





    // 적을 처리하는 메서드
    private void ProcessEnemy(Collider2D other)
    {

        Transform visionRange = other.transform.Find("VisionRange");

        if (visionRange != null)
        {
            
            EnemyVision enemyVision = visionRange.GetComponent<EnemyVision>();

            if (enemyVision != null)
            {

                enemyVision.ChangeVisionRange(2f); // 원하는 범위로 변경

            }

        }

    }





    // 적 원래대로 되돌리는 함수
    private void ReturnEnemy(Collider2D other)
    {

        // 적이 나가면 리스트에서 제거
        if (detectedEnemies.Contains(other))
        {

            detectedEnemies.Remove(other);

            // 적의 자식 오브젝트 중 VisionRange를 찾음
            Transform visionRange = other.transform.Find("VisionRange");

            if (visionRange != null)
            {

                EnemyVision enemyVision = visionRange.GetComponent<EnemyVision>();

                if (enemyVision != null)
                {

                    // 원래 범위로 되돌림
                    enemyVision.ChangeVisionRange(originalVisionRangeSizes[other]);
                    originalVisionRangeSizes.Remove(other); // originalVisionRangeSizes에서 제거

                }

            }
            
        }

    }




    // 콜라이더를 비활성화 후 일정 시간 뒤에 다시 활성화
    IEnumerator DisableColliderTemporarily(float duration)
    {
        
        // BoxCollider2D 비활성화
        boxCollider.enabled = false;

        // duration만큼 대기
        yield return new WaitForSeconds(duration);

        // BoxCollider2D 다시 활성화
        boxCollider.enabled = true;

    }

}
