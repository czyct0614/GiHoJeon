using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    public GameObject EnemyBullet; // 발사체 프리팹
    public Transform firePoint; // 발사 위치
    public float projectileSpeed = 5f; // 발사체 속도
    public float attackCooldown = 2f; // 공격 쿨다운
    private bool canAttack = true; // 공격 가능 여부
    private float attackRange = 20f;
    private Transform target; // 플레이어의 위치

    private void Start()
    {
        // 플레이어 오브젝트를 찾습니다. (적이 플레이어를 추적하는 방법에 따라 다를 수 있습니다.)
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // 적이 공격 가능하고 플레이어가 범위 내에 있으면 공격합니다.
        if (canAttack && target != null && Vector3.Distance(transform.position, target.position) < attackRange)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // 공격 쿨다운 설정
        canAttack = false;
        Invoke("ResetAttack", attackCooldown);

        // 발사체 생성
        GameObject projectile = Instantiate(EnemyBullet, firePoint.position, Quaternion.identity);

        // 발사체를 플레이어 방향으로 발사합니다.
        Vector2 direction = (target.position - transform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }

    private void ResetAttack()
    {
        // 공격 가능 상태로 변경
        canAttack = true;
    }
}
