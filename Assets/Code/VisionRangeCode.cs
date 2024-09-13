using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{

    private NewEnemyCode enemyScript;
    
    private bool attack = false;

    private PlayerMove playerScript;

    void Start()
    {

        enemyScript = GetComponentInParent<NewEnemyCode>();
        playerScript = Script.Find<PlayerMove>("Player");
        RelocateVisionRange();

    }





    void Update()
    {
    
        if (attack)
        {
            if (playerScript.isHided)
            {
                // 플레이어가 숨겨졌을 때 적의 시야 방향이 플레이어의 방향과 일치해야 함
                if (enemyScript.isFacingRight != playerScript.spriteRenderer.flipX)
                {
                    enemyScript.OnPlayerDetected();
                }
                else
                {
                    attack = false;
                    enemyScript.isPlayerDetected = false;
                }
            }
            else
            {
                enemyScript.OnPlayerDetected();
            }
        }

    }





    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 시야 범위에 들어옴!");

            // 적에게 플레이어를 추적하라고 알림
            attack = true;
        }

    }





    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 시야 범위에서 나감!");

            // 적에게 플레이어 추적을 중지하라고 알림
            attack = false;
            enemyScript.OnPlayerLost();
        }

    }





//시야 범위 크기 바꾸는 함수
    public void ChangeVisionRange(float rangeSize)
    {
        Vector3 OriginalScale = transform.localScale;

        OriginalScale.x = rangeSize;

        transform.localScale = OriginalScale;

        RelocateVisionRange();

    }





//시야 범위 위치 맞추는 함수
    public void RelocateVisionRange()
    {

        float rangeX = transform.localScale.x;

        // 이동할 x 좌표를 계산합니다.
        float newXPosition = (rangeX - 1) / 2;

        // 현재 오브젝트의 위치를 가져옵니다.
        Vector3 newPosition = transform.localPosition;

        // 새로운 x 좌표로 위치를 설정합니다.
        newPosition.x = newXPosition;

        // 오브젝트의 위치를 업데이트합니다.
        transform.localPosition = newPosition;
        Debug.Log(newPosition);

    }

}
