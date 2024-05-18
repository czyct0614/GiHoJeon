using UnityEngine;

public class PlatformEnemyController : MonoBehaviour
{
    public float moveSpeed = 2f; // 적의 이동 속도
    public float attackRange = 1.5f; // 공격 범위
    public float attackCooldown = 0.5f; // 공격 쿨다운
    public int attackDamage = 1; // 공격 데미지
    private Transform target; // 추적할 대상 (플레이어)
    private PlayerMove playerHealth; // 플레이어의 체력을 관리하는 스크립트
    private bool canAttack = true; // 공격 가능 여부
    Animator animator;
    float targetY;

    void Start()
    {
        // 플레이어를 타겟으로 설정
        target = GameObject.FindGameObjectWithTag("Player").transform;
        // 플레이어의 체력을 관리하는 스크립트 가져오기
        playerHealth = target.GetComponent<PlayerMove>();
        animator = GetComponent<Animator>();
        targetY = transform.position.y;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer <= attackRange && canAttack)
        {
            Attack();
        }
        else{
            animator.SetBool("Attack", false);
        }

    }

    void Attack()
    {
        // 공격 쿨다운 설정
        canAttack = false;
        Invoke("ResetAttack", attackCooldown);
        animator.SetBool("Attack", true);

        // 플레이어에게 피해 입힘
        playerHealth.TakeDamage(attackDamage);
    }

    void ResetAttack()
    {
        // 공격 가능 상태로 변경
        canAttack = true;
    }

    public void FollowPlayer()
    {
        // 플레이어를 향해 이동합니다.
        if(target != null){
            float targetX = target.position.x;
            float targetZ = target.position.z;
            Vector3 newPosition = new Vector3(targetX, targetY, targetZ);

            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
        }
    }
}
