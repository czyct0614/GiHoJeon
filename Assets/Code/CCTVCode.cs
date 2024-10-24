
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CCTVCode : MonoBehaviour
{

    private bool didThisEverChangedDangerRate = false;
    public bool hacked;
    public float cctvHackingDuration;
    private bool isHackingActivate;

    // CCTV의 원래 색상 저장
    private Color originalColor;

    // SpriteRenderer 컴포넌트 참조
    private SpriteRenderer spriteRenderer;

    public GameObject hackedPrefab;
    private Transform bodyTransform;



    void Start()
    {

        hacked = false;

        isHackingActivate = false;

        spriteRenderer = GetComponent<SpriteRenderer>();

        // 부모 오브젝트에서 CCTVBody 태그를 가진 자식 오브젝트의 Transform을 찾습니다.
        if (transform.parent != null)
        {
            bodyTransform = transform.parent.GetComponentsInChildren<Transform>()
                .FirstOrDefault(t => t.CompareTag("CCTVBody"));
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

    }




    void Update()
    {

        if (hacked && !isHackingActivate)
        {
            SetTransparency(0f);  // 투명도를 0.3으로 설정
            StartCoroutine(ResetAfterDelay());
        }

    }




    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (!isHackingActivate)
            {
                if (!didThisEverChangedDangerRate)
                {
                    Script.Find<DangerRate>("DangerBar").ChangeDangerRate(1);
                    didThisEverChangedDangerRate = true;
                }

            }

        }

    }




    private IEnumerator ResetAfterDelay()
    {

        isHackingActivate = true;

        GameObject hackedObject = Instantiate(hackedPrefab, bodyTransform.position, Quaternion.identity);

        yield return new WaitForSeconds(cctvHackingDuration);

        Destroy(hackedObject);

        hacked = false;

        ResetColor();

        isHackingActivate = false;

    }




    // 투명도 설정 함수
    private void SetTransparency(float alpha)
    {

        if (spriteRenderer != null)
        {
            Color newColor = spriteRenderer.color;
            newColor.a = alpha;  // 알파값을 수정하여 투명도 설정
            spriteRenderer.color = newColor;
        }

    }




    // 원래 색상으로 되돌리는 함수
    private void ResetColor()
    {

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;  // 원래 색상으로 복원
        }

    }

}
