using System.Collections;
using UnityEngine;

public class DashEnemyMove : MonoBehaviour
{
    PortalManager portalManager;
    Rigidbody2D rigid;
    public int nextMove; // 다음 행동지표를 결정할 변수
    Animator animator;
    SpriteRenderer spriteRenderer;
    public bool hacked = false;
    public float detectionRange = 5f;
    public float dashRange = 5f; // 대쉬 범위
    public float dashSpeed = 20f; // 대쉬 속도
    public float dashDelay = 1.5f; // 대쉬 준비 시간
    public GameObject dashRangeIndicatorPrefab; // 범위를 표시할 스프라이트 프리팹
    public LayerMask playerLayer; // 플레이어 레이어

    public int maxHealth = 3; // 몬스터의 최대 체력
    private int currentHealth; // 현재 체력
    private bool isDead = false; // 몬스터가 죽었는지 여부를 나타내는 변수
    private bool isDashing = false; // 대쉬 중인지 여부
    private bool HisDashing = false; // 대쉬 중인지 여부
    private bool isHackingInProgress = false; // 해킹 중인지 여부
    private Transform Gplayer;

    private GameObject dashRangeIndicator;
    private Coroutine dashCoroutine; // 대쉬 코루틴 참조를 저장하기 위한 변수
    public GameObject manaPrefab; // 마나 프리팹

    private void Start()
    {
        currentHealth = maxHealth; // 몬스터의 체력 초기화
        GameObject portalmanager = GameObject.Find("PortalManager"); // 포탈 찾기
        portalManager = portalmanager.GetComponent<PortalManager>();
        Gplayer = GameObject.FindGameObjectWithTag("Player").transform;

        // 범위 표시 스프라이트를 생성하고 비활성화
        dashRangeIndicator = Instantiate(dashRangeIndicatorPrefab, transform.position, Quaternion.identity);
        dashRangeIndicator.SetActive(false);
    }

    // 대미지를 받는 함수
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // 몬스터의 체력 감소

        if (currentHealth <= 0)
        {
            Die(); // 몬스터가 죽음
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    // 몬스터가 죽었을 때 호출되는 함수
    private void Die()
    {
        if (portalManager != null) // portalManager가 null이 아닌지 확인합니다.
        {
            // 죽음 처리 로직
            portalManager.MonsterDied(); // 포탈 매니저에 죽은 몬스터 수를 알림
            // 마나 드롭
            if (manaPrefab != null)
            {
                Instantiate(manaPrefab, transform.position, Quaternion.identity);
            }
            dashRangeIndicator.SetActive(false);
            Destroy(gameObject); // 몬스터 오브젝트 파괴
            // 몬스터가 죽었음을 알리고 상태를 변경함
            isDead = true;
        }
        else
        {
            Debug.LogError("EnemyMove 코드에서 포탈매니저가 정의되지 않았다고 함");
        }
    }

    private void Awake()
    {
        hacked = false;
        isHackingInProgress = false;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        Invoke("Think", 5); // 초기화 함수 안에 넣어서 실행될 때 마다(최초 1회) nextMove변수가 초기화 되도록함
    }

    private void FixedUpdate()
    {
        if (hacked && !isHackingInProgress)
        {
            StartCoroutine(DForceTurn());
        }

        if (hacked){
            // 대쉬 도중 해킹 발생시 현재 실행 중인 대쉬 중지
            StopCoroutine(DashSequence());
        }

        if (isDashing) return;

        // Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y); // nextMove에 0:멈춤 -1:왼쪽 1:오른쪽으로 이동

        // Platform check(맵 앞이 낭떠러지면 뒤돌기 위해서 지형을 탐색)
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.4f, rigid.position.y);

        // 한칸 앞 부분아래 쪽으로 ray를 쏨
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        // 레이를 쏴서 맞은 오브젝트를 탐지
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));

        // 탐지된 오브젝트가 null: 그 앞에 지형이 없음
        if (raycast.collider == null)
        {
            Turn();
        }

        float distanceToPlayer = Vector3.Distance(transform.position, Gplayer.position);

        if (distanceToPlayer <= detectionRange && !isDashing && !hacked)
        {
            CancelInvoke(); // think를 잠시 멈춘 후 재실행
            Invoke("Think", 2f);
            StartDashSequence();
        }
    }

    public void Think()
    {
        if (isDashing) return;

        // Set Next Active
        nextMove = Random.Range(-1, 2);

        // Sprite Animation
        animator.SetInteger("WalkSpeed", nextMove);

        // Flip Sprite
        if (nextMove != 0) // 서있을 때 굳이 방향을 바꿀 필요가 없음
            spriteRenderer.flipX = nextMove == 1; // nextMove가 1이면 방향을 반대로 변경

        // Recursive (재귀함수는 가장 아래에 쓰는게 기본적)
        float time = Random.Range(2f, 5f); // 생각하는 시간을 랜덤으로 부여
        Invoke("Think", time); // 매개변수로 받은 함수를 time초의 딜레이를 부여하여 재실행
    }

    void Turn()
    {
        nextMove = nextMove * (-1); // 우리가 직접 방향을 바꾸어 주었으니 Think는 잠시 멈추어야함
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke(); // think를 잠시 멈춘 후 재실행
        Invoke("Think", 2);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }

    // 대쉬 시퀀스를 시작하는 함수
    public void StartDashSequence()
    {
        if (isDashing) return;
        if(!hacked){
            // 기존에 실행 중인 대쉬 코루틴이 있다면 중지하고 새로운 코루틴을 시작
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            dashCoroutine = StartCoroutine(DashSequence());
        }
    }

    // 대쉬 시퀀스를 처리하는 코루틴
    IEnumerator DashSequence()
    {

        if (hacked)
        {
            isDashing = false; // 대쉬 중단
            yield break; // 코루틴 종료
        }

        isDashing = true;
        
        // 현재 움직임 멈춤
        rigid.velocity = Vector2.zero;
        animator.SetInteger("WalkSpeed", 0);
        CancelInvoke("Think"); // 대쉬 도중에는 Think 코루틴을 멈춥니다.

        // 플레이어 위치 저장
        Vector3 playerPosition = Gplayer.position;

        // 범위 표시
        dashRangeIndicator.SetActive(true);
        dashRangeIndicator.transform.position = transform.position;

        // 플레이어 방향으로 범위 표시
        Vector3 dashDirection = (playerPosition - transform.position).normalized;
        dashDirection.y = 0; // Y축 방향 제거
        playerPosition.y = 0;
        dashRangeIndicator.transform.rotation = Quaternion.FromToRotation(Vector3.right, playerPosition);
        dashRangeIndicator.transform.localScale = new Vector2(dashRange, 1.5f);
        dashRangeIndicator.transform.position += new Vector3(dashDirection.x, 0, 0);

        // 빨간색으로 표시
        SpriteRenderer dashRangeRenderer = dashRangeIndicator.GetComponent<SpriteRenderer>();
        dashRangeRenderer.color = new Color(1, 0, 0, 0.5f);

        yield return new WaitForSeconds(dashDelay);

        // 플레이어와 충돌 확인
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dashDirection, dashRange, playerLayer);
        if (hit.collider != null)
        {
            NewPlayerCode player = hit.collider.GetComponent<NewPlayerCode>(); // 플레이어 스크립트에 접근
            if (player != null)
            {
                player.TakeDamage(1); // 플레이어에게 대미지
            }
        }

        dashRangeIndicator.SetActive(false);

        // 돌진
        float dashDistance = dashRange;
        float dashTime = dashDistance / dashSpeed;

        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < dashTime)
        {
            transform.position = Vector3.Lerp(startingPosition, startingPosition + dashDirection * dashDistance, elapsedTime / dashTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    public IEnumerator DForceTurn()
    {
        isHackingInProgress = true;
        
        spriteRenderer.color = new Color(1, 0, 0, 1f);

        HisDashing = true;
        
        // 현재 움직임 멈춤
        rigid.velocity = Vector2.zero;
        animator.SetInteger("WalkSpeed", 0);
        CancelInvoke("Think"); // 대쉬 도중에는 Think 코루틴을 멈춥니다.

        // 플레이어 위치 저장
        Vector3 playerPosition = Gplayer.position;

        // 범위 표시
        dashRangeIndicator.SetActive(true);
        dashRangeIndicator.transform.position = transform.position;

        // !플레이어 방향으로 범위 표시
        Vector3 dashDirection = (playerPosition - transform.position).normalized;
        dashDirection.x = -dashDirection.x;
        dashDirection.y = 0; // Y축 방향 제거
        playerPosition.y = 0;
        dashRangeIndicator.transform.rotation = Quaternion.FromToRotation(Vector3.right, playerPosition);
        dashRangeIndicator.transform.localScale = new Vector2(dashRange, 1.5f);
        dashRangeIndicator.transform.position += new Vector3(dashDirection.x, 0, 0);

        // 빨간색으로 표시
        SpriteRenderer dashRangeRenderer = dashRangeIndicator.GetComponent<SpriteRenderer>();
        dashRangeRenderer.color = new Color(1, 0, 0, 0.5f);

        yield return new WaitForSeconds(dashDelay);

        dashRangeIndicator.SetActive(false);

        // 돌진
        float dashDistance = dashRange;
        float dashTime = dashDistance / dashSpeed;

        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < dashTime)
        {
            transform.position = Vector3.Lerp(startingPosition, startingPosition + dashDirection * dashDistance, elapsedTime / dashTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        HisDashing = false;
        spriteRenderer.color = new Color(0.1603774f, 0.1603774f, 0.1603774f, 1f); // 색상 복원
        isHackingInProgress = false;
        hacked = false;
    }
}