using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongEnemyMove : MonoBehaviour
{
    PortalManager portalManager;
    Rigidbody2D rigid;
    public int nextMove; //다음 행동지표를 결정할 변수
    Animator animator;
    SpriteRenderer spriteRenderer;
    public GameObject dropItemPrefab;
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform firePoint; // 발사 위치

    public int maxHealth = 3; // 몬스터의 최대 체력
    private int currentHealth; // 현재 체력
    private bool isDead = false; // 몬스터가 죽었는지 여부를 나타내는 변수
    public GameObject manaPrefab; // 마나 프리팹

    public bool hacked = false;
    
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
            FireInArc();
            Die(); // 몬스터가 죽음
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    // 해킹을 당했을 때 호출되는 함수
    public void OnHacked()
    {
        if (!isDead)
        {
            FireInArc();
            Die();
        }
    }

    // 부채꼴로 총알을 발사하는 함수
    private void FireInArc()
    {
        int bulletsToFire = 25; // 발사할 총알의 개수
        float spreadAngle = 180f; // 부채꼴 각도
        float angleStep = spreadAngle / (bulletsToFire - 1);
        float startAngle = 0;

        for (int i = 0; i < bulletsToFire; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = rotation * Vector2.right * 10f;
        }
    }

    // 몬스터가 죽었을 때 호출되는 함수
    private void Die()
    {
        GameObject dropItem = Instantiate(dropItemPrefab, transform.position, transform.rotation);
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
            Debug.LogError("PortalManager is not assigned!");
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        Invoke("Think", 5); // 초기화 함수 안에 넣어서 실행될 때 마다(최초 1회) nextMove변수가 초기화 되도록함 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y); //nextMove 에 0:멈춤 -1:왼쪽 1:오른쪽 으로 이동 

        // Platform check(맵 앞이 낭떨어지면 뒤돌기 위해서 지형을 탐색)
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.4f, rigid.position.y);

        // 한칸 앞 부분아래 쪽으로 ray를 쏨
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        // 레이를 쏴서 맞은 오브젝트를 탐지 
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));

        // 탐지된 오브젝트가 null : 그 앞에 지형이 없음
        if (raycast.collider == null)
        {
            Turn();
        }

        if (hacked){
            OnHacked();
        }
    }

    void Think()
    {
        // 몬스터가 스스로 생각해서 판단 (-1:왼쪽이동 ,1:오른쪽 이동 ,0:멈춤  으로 3가지 행동을 판단)
        nextMove = Random.Range(-1, 2);

        // Sprite Animation
        animator.SetInteger("WalkSpeed", nextMove);

        // Flip Sprite
        if (nextMove != 0) // 서있을 때 굳이 방향을 바꿀 필요가 없음 
            spriteRenderer.flipX = nextMove == 1; // nextmove 가 1이면 방향을 반대로 변경  

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
}
