using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyMove : MonoBehaviour
{
    PortalManager portalManager;
    Rigidbody2D rigid;
    public int nextMove; // 다음 행동지표를 결정할 변수
    Animator animator;
    SpriteRenderer spriteRenderer;
    public bool hacked = false;
    private Vector3 hackedPlayerPosition; // 해킹된 순간의 플레이어 위치

    public int maxHealth = 3; // 몬스터의 최대 체력
    private int currentHealth; // 현재 체력
    private bool isDead = false; // 몬스터가 죽었는지 여부를 나타내는 변수
    public GameObject manaPrefab; // 마나 프리팹
    public bool rush = false;
    public bool forceturn = false;
    public bool explosion = false;
    float explosionRadius = 2f;
    int explosionDamage = 1;
    private PlayerMove playerHealth; // 플레이어의 체력을 관리하는 스크립트
    
    private void Start()
    {
        currentHealth = maxHealth; // 몬스터의 체력 초기화
        GameObject portalmanager = GameObject.Find("PortalManager"); // 포탈 찾기
        portalManager = portalmanager.GetComponent<PortalManager>();
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
            Destroy(gameObject); // 몬스터 오브젝트 파괴
            // 몬스터가 죽었음을 알리고 상태를 변경함
            isDead = true;
        }
        else
        {
            Debug.LogError("EnemyMove 코드에서 포탈매니저가 정의되지 않았다고 함");
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        hacked = false;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        Invoke("Think", 5); // 초기화 함수 안에 넣어서 실행될 때 마다(최초 1회) nextMove변수가 초기화 되도록함
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (explosion && !rush)
        {
            StartCoroutine(HackedRush(0.5f));
        }

        if (hacked && !forceturn){
            StartCoroutine(SForceTurn(0.1f));
        }

        else
        {
            // Move
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y); // nextMove 에 0:멈춤 -1:왼쪽 1:오른쪽 으로 이동

            // Platform check(맵 앞이 낭떠러지면 뒤돌기 위해서 지형을 탐색)

            // 자신의 한 칸 앞 지형을 탐색해야하므로 position.x + nextMove(-1,1,0이므로 적절함)
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.4f, rigid.position.y);

            // 한칸 앞 부분 아래 쪽으로 ray를 쏨
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

            // 레이를 쏴서 맞은 오브젝트를 탐지
            RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));

            // 탐지된 오브젝트가 null : 그 앞에 지형이 없음
            if (raycast.collider == null)
            {
                Turn();
            }
        }
    }

    public void Think()
    {
        // 몬스터가 스스로 생각해서 판단 (-1:왼쪽이동 ,1:오른쪽 이동 ,0:멈춤 으로 3가지 행동을 판단)

        // Set Next Active
        // Random.Range : 최소<= 난수 <최대 /범위의 랜덤 수를 생성(최대는 제외이므로 주의해야함)
        nextMove = Random.Range(-1, 2);

        // Sprite Animation
        // WalkSpeed변수를 nextMove로 초기화
        animator.SetInteger("WalkSpeed", nextMove);

        // Flip Sprite
        if (nextMove != 0) // 서있을 때 굳이 방향을 바꿀 필요가 없음
            spriteRenderer.flipX = nextMove == 1; // nextMove가 1이면 방향을 반대로 변경

        // Recursive (재귀함수는 가장 아래에 쓰는게 기본적)
        float time = Random.Range(2f, 5f); // 생각하는 시간을 랜덤으로 부여
        // Think(); : 재귀함수 : 딜레이를 쓰지 않으면 CPU 과부화 되므로 재귀함수 쓸 때는 항상 주의 ->Think()를 직접 호출하는 대신 Invoke() 사용
        Invoke("Think", time); // 매개변수로 받은 함수를 time초의 딜레이를 부여하여 재실행
    }

    void Turn()
    {
        nextMove = nextMove * (-1); // 우리가 직접 방향을 바꾸어 주었으니 Think는 잠시 멈추어야함
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke(); // think를 잠시 멈춘 후 재실행
        Invoke("Think", 2);
    }

    public IEnumerator SForceTurn(float duration)
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
    }


    public IEnumerator HackedRush(float duration)
    {
        rush = true;
        Debug.Log("Hacked Rush");
        hackedPlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        spriteRenderer.color = new Color(1, 0, 0, 1f);

        // 해킹된 순간의 플레이어 위치를 기준으로 반대 방향으로 돌진
        Vector3 dashDirection = -(hackedPlayerPosition - transform.position).normalized;
        dashDirection.y = 0;
        Vector3 rushDirection = dashDirection;

        // 돌진
        float dashSpeed = 10f;
        float dashDistance = 20f;
        float dashTime = dashDistance / dashSpeed;

        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        yield return new WaitForSeconds(duration);

        while (elapsedTime < dashTime)
        {
            transform.position = Vector3.Lerp(startingPosition, startingPosition + rushDirection * dashDistance, elapsedTime / dashTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rigid.velocity = Vector2.zero; // 멈춤
        spriteRenderer.color = new Color(1, 1, 1, 1f); // 색상 복원
        hacked = false;
        rush = false;
    }

    void DeActive()
    {
        // 오브젝트 끄기
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (rush && other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("STOP");
            Explode();
            rigid.velocity = Vector2.zero; // 멈춤
            StopCoroutine(HackedRush(0.5f));
        }
    }

    private void Explode()
    {
        // 자폭 효과를 추가하고 플레이어에게 피해를 줌
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Player"))
            {
                playerHealth.TakeDamage(explosionDamage);
            }

            else if (nearbyObject.CompareTag("Monster"))
            {
                // 몬스터에게 대미지를 줌
                if(nearbyObject.gameObject.name=="BOSS"){
                    nearbyObject.GetComponent<BOSSMove>().TakeDamage(explosionDamage);
                }
                else if(nearbyObject.gameObject.name=="StrongEnemy"){
                    nearbyObject.GetComponent<StrongEnemyMove>().TakeDamage(explosionDamage);
                }
                else if(nearbyObject.gameObject.name=="PStrongEnemy"){
                    nearbyObject.GetComponent<PStrongEnemyMove>().TakeDamage(explosionDamage);
                }
                else if(nearbyObject.gameObject.name=="BaseEnemy"){
                    nearbyObject.GetComponent<EnemyMove>().TakeDamage(explosionDamage);
                }
                else if(nearbyObject.gameObject.name=="PlatformEnemy"){
                    nearbyObject.GetComponent<EnemyMove>().TakeDamage(explosionDamage);
                }
                else if(nearbyObject.gameObject.name=="ExplodingEnemy"){
                    nearbyObject.GetComponent<ExplodingEnemyMove>().TakeDamage(explosionDamage);
                }
                else if(nearbyObject.gameObject.name=="CurseEnemy"){
                    nearbyObject.GetComponent<CurseEnemyMove>().TakeDamage(explosionDamage);
                }

                Destroy(gameObject);
            }

        else if(nearbyObject.CompareTag("ShieldEnemy")){
                    nearbyObject.GetComponent<ShieldEnemyMove>().TakeDamage(explosionDamage);
                    Destroy(gameObject);
                }

        else if(nearbyObject.CompareTag("DashEnemy")){
                    nearbyObject.GetComponent<DashEnemyMove>().TakeDamage(explosionDamage);
                    Destroy(gameObject);
                }
        }

        // 자폭 후 적 파괴
        Die();
    }


}
