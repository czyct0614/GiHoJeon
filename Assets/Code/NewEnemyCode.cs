using UnityEngine;
using System.Collections;

public class NewEnemyCode : MonoBehaviour
{

    private SpriteRenderer playerSpriteRenderer;

    // 시야 범위 길이
    //public float visionRange = 5f;
    // 시야 범위 폭
    //public float visionWidth = 1f;

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

        player = GameObject.FindGameObjectWithTag("Player");
        
        playerScript = player.GetComponent<NewPlayerCode>();

        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        didThisEverChangedDangerRate = false;

        moveEndPoint = endPoint;

        sirenCode = Script.Find<SirenCode>("Siren");

        hacked = false;
        isHackingActivate = false;

    }





    void Update()
    {

        if (!hacked)
        {
            if (isPlayerDetected)
            {
                UpdateVisionDirectionWhileAttacking(moveEndPoint);
            }
            else
            {
                UpdateVisionDirection(moveEndPoint);
            }
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
            StopAllCoroutines(); // 기존 코루틴 중지
            StartCoroutine(FindPlayer(Script.Find<SoundCheckCode>("SoundCheck").lastPlayerPoint));
            return; // 다른 행동을 하지 않도록 함수 종료
        }



        if ((!isHeared && !isPlayerDetected && !findingPlayer && !sirenCode.ringing) || hacked)
        {
            Patrol();
        }

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

        StopAllCoroutines();
        ChaseAndAttackPlayer();

    }


    public void OnPlayerLost()
    {

        isPlayerDetected = false;
        attacking = false;

    }


    private void ChaseAndAttackPlayer()
    {

        if (!isPlayerDetected) return;

        Debug.Log("플레이어 추적 및 공격");

        if (hacked) return;

        patrolling = false;
        attacking = true;
        findingPlayer = false;

        // 플레이어를 향해 이동합니다.
        moveEndPoint = new Vector2(player.transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, moveEndPoint, Time.deltaTime * chaseSpeed);

        if (distanceToPlayer <= attackRange && canAttack)
        {
            StartCoroutine(AttackWithDelay());
        }
        
    }


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

        //animator.SetBool("Attack", true);

        // 플레이어에게 피해 입힘
        playerScript.TakeDamage(attackDamage);
        Debug.Log(attackDamage);

    }


    private void Patrol()
    {

        Debug.Log("순찰중..");

        moveEndPoint = endPoint;

        patrolling = true;

        // x축으로만 이동하도록 수정
        float newX = Mathf.MoveTowards(transform.position.x, moveEndPoint.x, Time.deltaTime * patrolSpeed);
        transform.position = new Vector2(newX, transform.position.y);

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

        while (Mathf.Abs(transform.position.x - lastPlayerPoint.x) > 0.01f)
        {
            if (isPlayerDetected)
            {
                findingPlayer = false;
                yield break;
            }

            float newX = Mathf.MoveTowards(transform.position.x, lastPlayerPoint.x, Time.deltaTime * soundTrackingSpeed);
            transform.position = new Vector2(newX, transform.position.y);
            
            yield return null; // 매 프레임마다 실행
        }

        // 1초간 대기
        yield return new WaitForSeconds(1f);

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
