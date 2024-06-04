using UnityEngine;

public class ShieldEnemyController : MonoBehaviour
{
    public float moveSpeed = 2f; // 적의 이동 속도
    public float attackRange = 1.5f; // 공격 범위
    public float attackCooldown = 0.5f; // 공격 쿨다운
    public int attackDamage = 1; // 공격 데미지
    public float detectRange = 10f; // 플레이어를 감지할 수 있는 최대 거리
    private Transform target; // 추적할 대상 (플레이어)
    private PlayerMove playerHealth; // 플레이어의 체력을 관리하는 스크립트
    private bool canAttack = true; // 공격 가능 여부
    ShieldEnemyMove SenemyMove;
    Animator animator;
    Transform shield; // 방패 오브젝트의 Transform

    void Start()
    {
        // 플레이어를 타겟으로 설정
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            target = player.transform;
            // 플레이어의 체력을 관리하는 스크립트 가져오기
            playerHealth = player.GetComponent<PlayerMove>();
        }
        else
        {
            Debug.LogWarning("EnemyAttack 코드에서 플레이어를 찾지 못하고 있음");
        }
        animator = GetComponent<Animator>();
        GameObject Senemy = GameObject.FindGameObjectWithTag("ShieldEnemy");
        SenemyMove = Senemy.GetComponent<ShieldEnemyMove>();

        // 방패 오브젝트의 Transform 가져오기
        shield = transform.Find("XShield1");
    }

    void Update()
    {
        if (target == null || playerHealth == null)
        {
            // target 또는 playerHealth가 null인 경우 업데이트를 중단합니다.
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer <= detectRange && !SenemyMove.hacked)
        {
            // 플레이어와의 거리가 일정 거리 이내에 있으면 플레이어를 추적합니다.
            FollowPlayer();

            if (distanceToPlayer <= attackRange && canAttack)
            {
                Attack();
            }

            else
            {
                animator.SetBool("Attack", false);
            }
        }

        if (distanceToPlayer <= detectRange && SenemyMove.hacked){
            //ifHacked();
        }

        // 방패를 플레이어 방향으로 향하도록 설정
        if (shield != null && !SenemyMove.hacked)
        {
            // 플레이어가 오른쪽에 있으면 방패를 오른쪽으로 반전시킵니다.
            if (target.position.x > transform.position.x)
            {
                shield.position = new Vector3(1.2f+transform.position.x, transform.position.y, 0);
            }
            // 플레이어가 왼쪽에 있으면 방패를 왼쪽으로 반전시킵니다.
            else
            {
                shield.position = new Vector3(-1.2f+transform.position.x, transform.position.y, 0);
            }
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

    private void FollowPlayer()
    {
        // 플레이어를 향해 이동합니다.
        float targetX = target.position.x;
        float targetY = -8f; // y축은 변경하지 않음
        float targetZ = target.position.z;
        Vector3 newPosition = new Vector3(targetX, targetY, targetZ);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }

    private void ifHacked()
    {
        // 플레이어 반대 방향으로 이동합니다.
        float targetX = target.position.x;
        float targetY = -8f; // y축은 변경하지 않음
        float targetZ = target.position.z;
        Vector3 newPosition = new Vector3(-targetX, targetY, targetZ);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
        if (target.position.x > transform.position.x)
            {
                shield.position = new Vector3(-1.2f+transform.position.x, transform.position.y, 0);
            }

            else
            {
                shield.position = new Vector3(1.2f+transform.position.x, transform.position.y, 0);
            }
    }
}
