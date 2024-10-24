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

    public GameObject hackedPrefab;

    private Coroutine currentFindPlayerCoroutine;

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
            UpdateVisionDirection(moveEndPoint);
        }



        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);



        if (isPlayerDetected && !hacked && !attacking)
        {
            StopCoroutine("Patrol");
            StopCoroutine("FindPlayer");
            ChaseAndAttackPlayer();
        }



        if (hacked && !isHackingActivate)
        {
            StartCoroutine(ResetAfterDelay());
        }



        if (isHeared && !isPlayerDetected && !hacked)
        {
            StopCoroutine("Patrol");
            if (currentFindPlayerCoroutine != null)
            {
                StopCoroutine(currentFindPlayerCoroutine);
            }
            currentFindPlayerCoroutine = StartCoroutine(FindPlayer(Script.Find<SoundCheckCode>("SoundCheck").lastPlayerPoint));
        }



        if (!isHeared && !isPlayerDetected && !findingPlayer && !sirenCode.ringing && !hacked)
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

        StopCoroutine("Patrol");
        StopCoroutine("FindPlayer");
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
        if (hacked) return;

        Debug.Log("플레이어 추적 및 공격");

        patrolling = false;
        attacking = true;
        findingPlayer = false;

        if (distanceToPlayer <= attackRange && canAttack)
        {
            StartCoroutine(AttackWithDelay());
        }

        // 플레이어를 향해 이동합니다.
        moveEndPoint = new Vector2(player.transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, moveEndPoint, Time.deltaTime * chaseSpeed);
        
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
            
            yield return new WaitForSeconds(0.001f); // 매 프레임마다 실행
        }

        // 2초간 대기
        yield return new WaitForSeconds(2f);

        // 다른 행동 실행
        findingPlayer = false;

        Debug.Log("플레이어를 찾지 못했습니다. 순찰을 재개합니다.");

        // 코루틴이 끝날 때 currentFindPlayerCoroutine을 null로 설정
        currentFindPlayerCoroutine = null;

    }


    private IEnumerator ResetAfterDelay()
    {

        isHackingActivate = true;

        GameObject hackedObject = Instantiate(hackedPrefab, transform.position, transform.rotation);

        hackedObject.transform.SetParent(transform);

        yield return new WaitForSeconds(newEnemyHackingDuration);

        Destroy(hackedObject);

        hacked = false;

        isHackingActivate = false;

    }

}
