using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 10f;  // 미사일 이동 속도
    private float lifetime = 5f;  // 미사일 수명 (초)
    private float spawnTime;  // 미사일 생성 시간
    public int damageAmount = 1; // 탄환의 대미지량

    void Start()
    {
        spawnTime = Time.time;  // 현재 시간을 저장
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

    private void OnTriggerEnter2D(Collider2D other){   
    
        if (other.CompareTag("Shield")){
            Destroy(gameObject);
        }
        // 몬스터에게 대미지를 줌
        if(other.CompareTag("BOSS")){
            other.GetComponent<BOSSMove>().TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        else if(other.CompareTag("StrongEnemy") || other.CompareTag("VeryStrongEnemy")){
            other.GetComponent<StrongEnemyMove>().TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        else if(other.CompareTag("PStrongEnemy")){
            other.GetComponent<PStrongEnemyMove>().TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        else if(other.CompareTag("Enemy") || other.CompareTag("PlatformEnemy")){
            other.GetComponent<EnemyMove>().TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        else if(other.CompareTag("ExplodingEnemy")){
            other.GetComponent<ExplodingEnemyMove>().TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        else if(other.CompareTag("CurseEnemy")){
            other.GetComponent<CurseEnemyMove>().TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        else if(other.CompareTag("TurretEnemy")){
            other.GetComponent<TurretMove>().TakeDamage(damageAmount);
            Destroy(gameObject);
        }

        else if(other.CompareTag("ShieldEnemy")){
                    other.GetComponent<ShieldEnemyMove>().TakeDamage(damageAmount);
                    Destroy(gameObject);
                }

        else if(other.CompareTag("DashEnemy")){
                    other.GetComponent<DashEnemyMove>().TakeDamage(damageAmount);
                    Destroy(gameObject);
                }

        else if(other.gameObject.name!="Player" && 
                !other.CompareTag("Item") && 
                !other.CompareTag("Portal") && 
                !other.CompareTag("FlyingPlatform") && 
                !other.CompareTag("Range") && 
                !other.CompareTag("Skill") && 
                !other.CompareTag("Ladder") && 
                !other.CompareTag("SpawnPoint") && 
                !other.CompareTag("Else")){

                    Destroy(gameObject);
                    
                }
   }
}
