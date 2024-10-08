using UnityEngine;
using System.Collections;

public class NewEnemyCode : MonoBehaviour
{

    private SpriteRenderer playerSpriteRenderer;

    // 시야 범위 길이
    //public float visionRange = 5f;
    // 시야 범위 폭
    //public float visionWidth = 1f;
    public float moveSpeed = 1f;
    public float attackMoveSpeed = 3f;
    public float patrolSpeed = 1f; // 기본 순찰 속도
    public float soundTrackingSpeed = 2f; // 소리 추적 속도
    public float chaseSpeed = 3f; // 플레이어 추격 속도
    public float newEnemyHackingDuration;
    private float distanceToPlayer;
    private float attackRange = 1.5f;
    private float attackCooldown = 2f;

    private int attackDamage = 5;

    // 플레이어 레이어
    public LayerMask playerLayer;
    private Transform player;
    private GameObject player;
    private NewPlayerCode playerScript;
    private SirenCode sirenCode;
    public GameObject visionObject;
    public Vector2 startPoint;
    public Vector2 endPoint;
    public Vector2 moveEndPoint;

    public bool isFacingRight = true;
    public bool isPlayerDetected = false;
    private bool attacking = false;
    private bool flipping = false;
    public bool isHeared = false;
    public bool patrolling = false;
    private bool didThisEverChangedDangerRate = false;
    public bool findingPlayer = false;
    public bool hacked;
    private bool isHackingActivate;
    private bool canAttack = true;

    void Start()
    {
        
        startPoint.y = transform.position.y;
        endPoint.y = transform.position.y;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        
        playerScript = player.GetComponent<NewPlayerCode>();

        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        didThisEverChangedDangerRate=false;

        moveEndPoint = endPoint;

        sirenCode = Script.Find<SirenCode>("Siren");

        hacked = false;
        isHackingActivate = false;

    }





    void Update()
    {

        if (hacked && !isHackingActivate)
        if (isPlayerDetected)
        {
            UpdateVisionDirectionWhileAttacking(moveEndPoint);
        }
        else
        {
            UpdateVisionDirection(moveEndPoint);
        }



        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);



        if (isPlayerDetected && !hacked)
        {
            StopAllCoroutines();
            ChaseAndAttackPlayer();
            return;
        }



        if (hacked && !isHackingActivate)
        {
            StartCoroutine(ResetAfterDelay());
        }



        if (isHeared && !isPlayerDetected && !hacked)
        {
            StartCoroutine(FindPlayer(Script.Find<SoundCheckCode>("SoundCheck").lastPlayerPoint));
        }


        
        if ((!isHeared && !isPlayerDetected && !findingPlayer && !sirenCode.ringing) || hacked)
        {
            if (!patrolling)
            {
                moveEndPoint = endPoint;
            }

            Patrol();
        }

        // 이동 방향에 따라 시야 범위 회전
        UpdateVisionDirection(moveEndPoint);

        distanceToPlayer = Vector3.Distance(transform.position, player.position);

    }





    void UpdateVisionDirection(Vector2 moveEndPoint)
    {

        if (moveEndPoint.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (moveEndPoint.x < transform.position.x && isFacingRight)
        {
            Flip();
        }

    }




    void UpdateVisionDirectionWhileAttacking(Vector2 moveEndPoint)
    {
        // 플레이어가 오른쪽을 바라보고 있고 (flipX가 false), 적이 왼쪽을 바라보고 있다면 (isFacingRight가 false)
        if (!playerSpriteRenderer.flipX && !isFacingRight)
        {
            Flip();
        }
        // 플레이어가 왼쪽을 바라보고 있고 (flipX가 true), 적이 오른쪽을 바라보고 있다면 (isFacingRight가 true)
        else if (playerSpriteRenderer.flipX && isFacingRight)
        {
            Flip();
        }
    }





    void Flip()
    {

        if (!flipping)
        {
            Debug.Log("회전");

            flipping = true;
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
            flipping = false;
        }

    }





    public void OnPlayerDetected()
    {

        if (!didThisEverChangedDangerRate)
        {
            Script.Find<DangerRate>("DangerBar").ChangeDangerRate(1);
            didThisEverChangedDangerRate = true;
        }

        isPlayerDetected = true;

        AttackPlayer();
        StopAllCoroutines();
        ChaseAndAttackPlayer();

    }





    public void OnPlayerLost()
    {

        isPlayerDetected = false;
        attacking = false;

    }





    private void AttackPlayer()
    private void ChaseAndAttackPlayer()
    {
        Debug.Log("공격");

        if (!isPlayerDetected) return;
        Debug.Log("플레이어 추적 및 공격");

        if (hacked) return;

        patrolling = false;
        attacking = true;
        findingPlayer = false;

        // 플레이어를 향해 이동합니다.
        moveEndPoint = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, moveEndPoint, Time.deltaTime * attackMoveSpeed);
        moveEndPoint = new Vector2(player.transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, moveEndPoint, Time.deltaTime * chaseSpeed);

        if (distanceToPlayer <= attackRange && canAttack)
        {
            if (!isFirstAttack)
            {
                Attack();
            }
            else
            {
                // 처음 공격 시 쿨타임 설정
                isFirstAttack = false;
                canAttack = false;
                Invoke("ResetAttack", attackCooldown);
            }
        }

    }





<<<<<<< Updated upstream
    void Attack()
    {
        // 공격 쿨다운 설정
        canAttack = false;
        Invoke("ResetAttack", attackCooldown);
=======
    IEnumerator AttackWithDelay()
    {

        canAttack = false;
        
        // 선딜레이 추가
        yield return new WaitForSeconds(0.5f);

        // 선딜레이 후 플레이어가 여전히 공격 범위 내에 있는지 확인
        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else
        {
            Debug.Log("플레이어가 공격 범위를 벗어났습니다. 공격 취소.");
        }
        
        yield return new WaitForSeconds(attackCooldown);
        
        canAttack = true;

    }





    void Attack()
    {
        
>>>>>>> Stashed changes
        //animator.SetBool("Attack", true);

        // 플레이어에게 피해 입힘
        playerScript.TakeDamage(attackDamage);

    }





<<<<<<< Updated upstream
    void ResetAttack()
    {
        // 공격 가능 상태로 변경
        canAttack = true;
    }





=======
>>>>>>> Stashed changes
    private void Patrol()
    {

        Debug.Log("순찰중..");

        moveEndPoint = endPoint;

        patrolling = true;

<<<<<<< Updated upstream
        transform.position = Vector2.MoveTowards(transform.position, moveEndPoint, Time.deltaTime * moveSpeed);
=======
        // x축으로만 이동하도록 수정
        float newX = Mathf.MoveTowards(transform.position.x, moveEndPoint.x, Time.deltaTime * patrolSpeed);
        transform.position = new Vector2(newX, transform.position.y);
>>>>>>> Stashed changes

        if (transform.position.x == endPoint.x)
        {
            moveEndPoint = startPoint;
        }

        else if (transform.position.x == startPoint.x)
        {
            moveEndPoint = endPoint;
        }

    }




    public IEnumerator FindPlayer(Vector2 lastPlayerPoint)
    {

        Debug.Log("소리 감지");

        patrolling = false;
        findingPlayer = true;
        moveEndPoint = lastPlayerPoint;

        if (isPlayerDetected)
        {
            findingPlayer = false;
            Debug.Log("Break");
            yield break;
        }



        // 플레이어의 마지막 위치로 이동
        while (Mathf.Abs(transform.position.x - lastPlayerPoint.x) > 0.01f)
        {
<<<<<<< Updated upstream
            transform.position = Vector2.MoveTowards(transform.position, lastPlayerPoint, Time.deltaTime * moveSpeed);
=======
            float newX = Mathf.MoveTowards(transform.position.x, lastPlayerPoint.x, Time.deltaTime * soundTrackingSpeed);
            transform.position = new Vector2(newX, transform.position.y);
            
            // 0.5초 대기
>>>>>>> Stashed changes
            yield return new WaitForSeconds(0.05f);

            if (patrolling)
            {
                findingPlayer = false;
                break;
                yield break;
            }
        }

        // 1초간 대기
        yield return new WaitForSeconds(1f);


        if (Mathf.Abs(transform.position.x - lastPlayerPoint.x) < 0.01f)
        {
            yield return new WaitForSeconds(3f);

            findingPlayer = false;
        }
        // 다른 행동 실행
        findingPlayer = false;
        patrolling = true;
        Debug.Log("플레이어를 찾지 못했습니다. 순찰을 재개합니다.");

    }





    private IEnumerator ResetAfterDelay()
    {

        isHackingActivate = true;

        yield return new WaitForSeconds(newEnemyHackingDuration);

        hacked = false;

        isHackingActivate = false;

    }

}