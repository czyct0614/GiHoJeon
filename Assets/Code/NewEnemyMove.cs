using UnityEngine;
using System.Collections;

public class NewEnemy : MonoBehaviour
{

    // 시야 범위 길이
    public float visionRange = 5f;
    // 시야 범위 폭
    public float visionWidth = 1f;
    public float moveSpeed = 1f;
    public float attackMoveSpeed = 3f;

    // 플레이어 레이어
    public LayerMask playerLayer;
    private Transform player;
    private PlayerMove playerScript;
    public GameObject visionObject;
    public Vector2 startPoint;
    public Vector2 endPoint;
    public Vector2 moveEndPoint;

    private bool isFacingRight = true;
    private bool isPlayerDetected = false;
    private bool attacking = false;
    private bool flipping = false;
    public bool isHeared = false;
    public bool patrolling = false;
    private bool didThisEverChangedDangerRate = false;
    public bool findingPlayer = false;

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<PlayerMove>();
        visionObject.transform.localPosition = new Vector3(3, 0, 0);
        didThisEverChangedDangerRate=false;
        moveEndPoint = endPoint;

    }





    void Update()
    {

        if (isHeared && !isPlayerDetected)
        {
            StartCoroutine(FindPlayer(Script.Find<SoundCheckCode>("SoundCheck").lastPlayerPoint));
            return;
        }


        
        if (!isHeared && !isPlayerDetected && !findingPlayer)
        {
            if (!patrolling)
            {
                moveEndPoint = endPoint;
            }

            Patrol();
        }



        if (!findingPlayer)
        {
            // 이동 방향에 따라 시야 범위 회전
            UpdateVisionDirection(moveEndPoint);
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
            flipping = true;
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;

            // 시야 범위 오브젝트도 회전
            visionObject.transform.localPosition = new Vector3(3, 0, 0);
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





    private void FollowPlayer()
    {

        patrolling = false;

        // 플레이어를 향해 이동합니다.
        transform.position = Vector2.MoveTowards(transform.position, moveEndPoint, Time.deltaTime * moveSpeed);
        Debug.Log("이동");

    }





    private void AttackPlayer()
    {

        if (!isPlayerDetected) return;

        patrolling = false;
        attacking = true;

        // 플레이어를 향해 이동합니다.
        moveEndPoint = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, moveEndPoint, Time.deltaTime * attackMoveSpeed);
        Debug.Log("공격");

    }





    private void Patrol()
    {

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

        patrolling = false;
        findingPlayer = true;

        if (isPlayerDetected)
        {
            findingPlayer = false;
            Debug.Log("Break");
            yield break;
        }



        while (Mathf.Abs(transform.position.x - lastPlayerPoint.x) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, lastPlayerPoint, Time.deltaTime * moveSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        Flip();

        yield return new WaitForSeconds(1f);
        Flip();

        findingPlayer = false;
        yield break;

    }

}