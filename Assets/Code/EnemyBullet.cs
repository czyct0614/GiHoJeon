using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f;  // 미사일 이동 속도
    private float lifetime = 5f;  // 미사일 수명 (초)
    private float spawnTime;  // 미사일 생성 시간
    public int damageAmount = 2; // 탄환의 대미지량
    private PlayerMove playerHealth; // 플레이어의 체력을 관리하는 스크립트
    private Transform target; // 추적할 대상 (플레이어)

    void Start()
    {
        spawnTime = Time.time;  // 현재 시간을 저장
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = target.GetComponent<PlayerMove>();
    }

    void Update()
    {
        // 미사일의 수명이 다 되면 삭제
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 Direction, float Speed)
    {
    // 주어진 방향이 (0, 0)이 아니라면
    if (Direction != Vector2.zero)
    {
        // 방향을 정규화하여 단위 벡터로 만듭니다.
        Direction.Normalize();

        // 객체가 주어진 방향을 향하도록 회전합니다.
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Rigidbody2D에 힘을 가해 방향으로 날아가게 합니다.
    GetComponent<Rigidbody2D>().AddForce(Direction * Speed);
}       
    private void OnTriggerEnter2D(Collider2D other)
   {   
    // 충돌한 오브젝트가 몬스터인 경우
    if (other.gameObject.name!="StrongEnemy" && !other.CompareTag("Monster"))
        {
            // 몬스터에게 대미지를 줌
            if(other.gameObject.name=="Player"){
                playerHealth.TakeDamage(damageAmount);
                Destroy(gameObject);
            }

        }
    else if(other.gameObject.name!="StrongEnemy" && !other.CompareTag("Item") && !other.CompareTag("Portal") && !other.CompareTag("Monster") && !other.CompareTag("FlyingPlatform")){
                Debug.Log("적 총알이 무언가에 닿아 없어짐");
                Destroy(gameObject);
            }
   }
}
