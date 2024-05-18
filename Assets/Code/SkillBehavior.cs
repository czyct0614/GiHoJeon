using System.Collections;
using UnityEngine;

public class SkillBehavior : MonoBehaviour
{
    private float lifetime = 5f;        // 미사일 수명 (초)
    public int damageAmount = 3;       // 탄환의 대미지량
    public TrailRenderer tr;           // 대쉬 잔상을 남기는 트레일 렌더러
    public PlayerMove move;

    // Start is called before the first frame update
    void Start()
    {
        // 대쉬 잔상 초기에 비활성화
        tr.emitting = false;
        move=ScriptFinder.FindScriptWithTag<PlayerMove>("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // 대쉬 버튼이 눌렸을 때 미사일 파괴 코루틴 시작
        if (Input.GetButtonUp("Charge"))
        {
            StartCoroutine(DestroyMissile());
        }

        // 대쉬 버튼이 눌리지 않은 경우 대쉬 잔상 활성화
        if (!Input.GetButton("Charge"))
        {
            tr.emitting = true;
        }else if(move.isDying)
        {
            Destroy(gameObject);
        }
    }
    
    // 미사일 파괴 코루틴
    IEnumerator DestroyMissile()
    {
        // 일정 시간 후에 미사일을 파괴합니다.
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    // 충돌 감지
    private void OnTriggerEnter2D(Collider2D other)
   {   
        // 대쉬 중이 아니면서 몬스터와 충돌한 경우
        if (!Input.GetButton("Charge") && other.CompareTag("Monster") && !other.CompareTag("FlyingPlatform") && other.gameObject.name != "Player")
        {
            // 몬스터에게 대미지를 줌
            if (other.gameObject.name == "BOSS")
            {
                other.GetComponent<BOSSMove>().TakeDamage(damageAmount);
            }
            else if (other.gameObject.name == "StrongEnemy")
            {
                other.GetComponent<StrongEnemyMove>().TakeDamage(damageAmount);
            }
            else
            {
                other.GetComponent<EnemyMove>().TakeDamage(damageAmount);
            }
        }
    }
}
