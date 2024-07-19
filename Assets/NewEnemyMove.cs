using UnityEngine;

public class NewEnemy : MonoBehaviour
{
    public float visionRange = 5f;      // 시야 범위 길이
    public float visionWidth = 1f;      // 시야 범위 폭
    public float moveSpeed = 1f;
    public float attackMoveSpeed = 3f;
    public LayerMask playerLayer;       // 플레이어 레이어

    private Transform player;
    private PlayerMove playerScript;
    public GameObject visionObject;
    private bool isFacingRight = true;
    private bool isPlayerDetected = false;
    private bool attacking = false;
    private bool flipping = false;
    public bool isHeared = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<PlayerMove>();
        visionObject.transform.localPosition = new Vector3(3, 0, 0);
    }

    void Update()
    {
        if (player == null) return;

        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // 소리 범위 체크
        if (isHeared && !attacking)
        {
            Debug.Log("플레이어가 소리 범위에 들어옴!");
            FollowPlayer();
        }

        // 이동 방향에 따라 시야 범위 회전
        UpdateVisionDirection(directionToPlayer);
    }

    void UpdateVisionDirection(Vector2 directionToPlayer)
    {
        if (directionToPlayer.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (directionToPlayer.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        if(!flipping){
            flipping = true;
            Debug.Log("Flip");
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
        // 플레이어를 향해 이동합니다.
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        Debug.Log("이동");
    }

    private void AttackPlayer()
    {
        if (!isPlayerDetected) return;

        // 플레이어를 향해 이동합니다.
        attacking = true;
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, attackMoveSpeed * Time.deltaTime);
        Debug.Log("공격");
    }
/*
    private void Awake() {
        hacked = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        Invoke("Think", 5); // 초기화 함수 안에 넣어서 실행될 때 마다(최초 1회) nextMove변수가 초기화 되도록함 
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(hacked && !forceturn){
            StartCoroutine(NewEnemyForceTurn(0.05f));
        }
        //Move
       rigid.velocity = new Vector2(nextMove,rigid.velocity.y); //nextMove 에 0:멈춤 -1:왼쪽 1:오른쪽 으로 이동 


       //Platform check(맵 앞이 낭떨어지면 뒤돌기 위해서 지형을 탐색)


       //자신의 한 칸 앞 지형을 탐색해야하므로 position.x + nextMove(-1,1,0이므로 적절함)
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.4f, rigid.position.y);

        //한칸 앞 부분아래 쪽으로 ray를 쏨
        Debug.DrawRay(frontVec, Vector3.down, new Color(0,1,0));

        //레이를 쏴서 맞은 오브젝트를 탐지 
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down,2,LayerMask.GetMask("Platform"));

        //탐지된 오브젝트가 null : 그 앞에 지형이 없음
        if(raycast.collider == null){
            Turn();
        }

    }


    public void Think(){//몬스터가 스스로 생각해서 판단 (-1:왼쪽이동 ,1:오른쪽 이동 ,0:멈춤  으로 3가지 행동을 판단)

        //Set Next Active
        //Random.Range : 최소<= 난수 <최대 /범위의 랜덤 수를 생성(최대는 제외이므로 주의해야함)
        nextMove = Random.Range(-1,2);


        //Flip Sprite
        if(nextMove != 0) //서있을 때 굳이 방향을 바꿀 필요가 없음 
            spriteRenderer.flipX = nextMove == 1; //nextmove 가 1이면 방향을 반대로 변경  


        //Recursive (재귀함수는 가장 아래에 쓰는게 기본적) 
        float time = Random.Range(2f, 5f); //생각하는 시간을 랜덤으로 부여 
        //Think(); : 재귀함수 : 딜레이를 쓰지 않으면 CPU과부화 되므로 재귀함수쓸 때는 항상 주의 ->Think()를 직접 호출하는 대신 Invoke()사용
        Invoke("Think", time); //매개변수로 받은 함수를 time초의 딜레이를 부여하여 재실행 
    }

    void Turn(){

        nextMove= nextMove*(-1); //우리가 직접 방향을 바꾸어 주었으니 Think는 잠시 멈추어야함
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke(); //think를 잠시 멈춘 후 재실행
        Invoke("Think",2);

    }

    private IEnumerator NewEnemyForceTurn(float duration)
    {
        forceturn = true;
        Debug.Log("hacked");
        hackedPlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        spriteRenderer.color = new Color(1, 0, 0, 1f);

        yield return new WaitForSeconds(duration); // 지정된 시간 동안 대기

        float dashSpeed = 3f;
        float dashDistance = 5f;
        float dashTime = dashDistance / dashSpeed;

        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        Vector3 dashDirection = -(hackedPlayerPosition - transform.position).normalized;
        dashDirection.y = 0;
        Vector3 rushDirection = dashDirection;

        yield return new WaitForSeconds(duration);

        while (elapsedTime < dashTime)
        {
            transform.position = Vector3.Lerp(startingPosition, startingPosition + rushDirection * dashDistance, elapsedTime / dashTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 원래 방향으로 돌아감
        nextMove = nextMove * -1; // 방향을 다시 반대로 바꿈
        spriteRenderer.flipX = nextMove == 1;
        spriteRenderer.color = new Color(1, 1, 1, 1f); // 색상 복원
        hacked = false;
        forceturn = false;
    }*/
}
