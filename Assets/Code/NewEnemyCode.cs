using UnityEngine;
using System.Collections;

public class NewEnemyCode : MonoBehaviour
{

    // 시야 범위 길이
    //public float visionRange = 5f;
    // 시야 범위 폭
    //public float visionWidth = 1f;
    public float moveSpeed = 1f;
    public float attackMoveSpeed = 3f;
    public float newEnemyHackingDuration;

    // 플레이어 레이어
    public LayerMask playerLayer;
    private Transform player;
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

    void Start()
    {
        startPoint.y = transform.position.y;
        endPoint.y = transform.position.y;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        playerScript = player.GetComponent<NewPlayerCode>();

        didThisEverChangedDangerRate=false;

        moveEndPoint = endPoint;

        sirenCode = Script.Find<SirenCode>("Siren");

        hacked = false;
        isHackingActivate = false;

    }





    void Update()
    {

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

        AttackPlayer();

    }





    public void OnPlayerLost()
    {

        isPlayerDetected = false;
        attacking = false;

    }





    private void AttackPlayer()
    {

        Debug.Log("공격");

        if (!isPlayerDetected) return;
        if (hacked) return;
        patrolling = false;
        attacking = true;

        // 플레이어를 향해 이동합니다.
        moveEndPoint = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, moveEndPoint, Time.deltaTime * attackMoveSpeed);

    }





    private void Patrol()
    {

        Debug.Log("순찰중..");

        patrolling = true;

        transform.position = Vector2.MoveTowards(transform.position, moveEndPoint, Time.deltaTime * moveSpeed);

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



        while (Mathf.Abs(transform.position.x - lastPlayerPoint.x) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, lastPlayerPoint, Time.deltaTime * moveSpeed);
            yield return new WaitForSeconds(0.05f);

            if (patrolling)
            {
                findingPlayer = false;
                break;
            }
        }



        if (Mathf.Abs(transform.position.x - lastPlayerPoint.x) < 0.01f)
        {
            yield return new WaitForSeconds(3f);

            findingPlayer = false;
        }

    }





    private IEnumerator ResetAfterDelay()
    {

        isHackingActivate = true;

        yield return new WaitForSeconds(newEnemyHackingDuration);

        hacked = false;

        isHackingActivate = false;

    }

}