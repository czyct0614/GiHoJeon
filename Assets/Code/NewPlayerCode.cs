using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewPlayerCode : MonoBehaviour
{



    [Header("====>모션<====")]

    // 방향 전환을 위한 변수
    public SpriteRenderer spriteRenderer;
    // 애니메이터 조작을 위한 변수
    private Animator animator;
    // 달리기 모션
    private bool isRunning = false;



    [Header("====>체력<====")]

    // 최대 체력
    public int maxHealth = 10;
    // 현재 체력
    public int currentHealth = 10;



    [Header("====>이동<====")]

    // 물리 이동을 위한 변수 선언
    private Rigidbody2D rigid;
    // 최대 속력 변수 
    public float maxSpeed;
    public float isFlipped;
    public bool isFastRunning;
    public bool isCrouching;
    private bool velocityZero = false;
    


    [Header("====>죽음/부활<====")]

    public Vector2 lastSpawnPoint;
    // 부활 무적시간
    public float invincibleDuration = 0.5f;
    public string nowMap;
    public bool dead = false;
    // 죽고 있는지
    public bool isDying = false;
    public bool canInput = true;



    [Header("====>기타<====")]

    public bool isHided = false;

    private GameObject player;
    private GameObject soundRange;

    // 무적 상태 여부를 나타내는 변수
    private bool isInvincible = false;

    private static bool playerExists = false;

    private Collider2D playerCollider;

    Vector2 originPos = new Vector2();

    public int soundAmount = 0;

    private GameObject soundCheck;

    private SoundCheckCode soundCheckCode;

    private GameObject newEnemy;

    private NewEnemyCode newEnemyCode;





//시작 함수
    void Awake() 
    {

        // 맵 바꿔도 안 날아가게
        DontDestroyOnLoad(this.gameObject);

        soundRange = GameObject.FindGameObjectWithTag("SoundRange");

        nowMap = SceneManager.GetActiveScene().name;

        player = GameObject.FindGameObjectWithTag("Player");

        newEnemy = GameObject.FindGameObjectWithTag("NewEnemy");

        if(newEnemy!=null)
        {
            newEnemyCode = newEnemy.GetComponent<NewEnemyCode>();
        }

        soundCheck = GameObject.FindGameObjectWithTag("SoundCheck");

        if(soundCheck!=null)
        {
            soundCheckCode = soundCheck.GetComponent<SoundCheckCode>();
        }

        Time.timeScale = 1f;

        // 게임 시작하면 위치 저장
        originPos = transform.position;

        // 변수 초기화
        rigid = GetComponent<Rigidbody2D>();

        // 초기화
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();

        //currentHealth = maxHealth;

        //ChangeHealthBarAmount();

        playerCollider = GetComponent<Collider2D>();



        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }



        // 부활무적 시간
        invincibleDuration = 0.5f;

        dead = false;

        // 죽고 있는지
        isDying = false;

        canInput = true;

        // 무적 상태 여부를 나타내는 변수
        isInvincible = false;

        isHided = false;
        
        maxSpeed=5f;

        isFastRunning = false;
        isCrouching = false;

    }



//업데이트 함수
    void Update() 
    {

        if (Input.GetMouseButtonDown(0)) 
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Clicked position: " + clickPosition);
        }



        if (Input.GetButton("left") || Input.GetButton("right") || Input.GetButton("Run") || Input.GetButton("Interact"))
        {
            ActivateSoundRange();
        }
        else 
        {
            DeactivateSoundRange();
        }



        if (Input.GetButton("left") || Input.GetButton("right"))
        {
            soundAmount = 5;
        }



        if (Input.GetButton("Run"))
        {
            soundAmount = 10;
        }



        if (Input.GetButton("Interact"))
        {
            soundAmount = 7;
        }



        if (Input.GetButton("Kill"))
        {
            soundAmount = 5;
            Kill();
        }



        if (Input.GetButton("Skill"))
        {
            soundAmount = 5;
        }



        if (nowMap != SceneManager.GetActiveScene().name)
        {
            if (SceneManager.GetActiveScene().name == "StartScene")
            {
                transform.position = new Vector3(0,-3,-10);
                rigid.gravityScale = 0f;
                isDying = true;
            }
            else
            {
                transform.position = new Vector3(transform.position.x,transform.position.y,0f);
                rigid.gravityScale = 8f;
                isDying = false;
            }
            
            nowMap = SceneManager.GetActiveScene().name;
        }
    


//죽고있을떄 다른 행동 못하게
        if (isDying)
        {
            return;
        }



        lastSpawnPoint = PlayerRoomManager.Instance.GetLastTouchedSpawnPoint();



// 주인공 콜라이더 불러오기
        if (playerCollider == null)
        {
            playerCollider = GetComponent<Collider2D>();
        }



// 주인공 물리엔진 불러오기
        if (rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }



// 주인공 스프라이트 렌더러 불러오기
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }



// 주인공 애니메이터 불러오기
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }



// 은신시 아무 행동 못하게
        if (isHided == true)
        {
            return;
        }



// 캐릭터 위치
        Vector2 PlayerPos = GetComponent<Rigidbody2D>().position;
        


// 마우스 위치
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);



// 낙사
        if (dead)
        {
            OnDie();
            // 죽고 난 뒤 가속도 초기화하기 (다시 죽는것 방지)
            rigid.velocity = new Vector2(rigid.velocity.x , 0);
        }



        if (Input.GetButtonUp("left") || Input.GetButtonUp("right"))
        {
            rigid.velocity = new Vector2( 0f * rigid.velocity.normalized.x , rigid.velocity.y);
        }



// 속도 낮을때 멈추기
        if (Mathf.Abs(rigid.velocity.x) < 0.8) // 속도가 0 == 멈춤
        {
            animator.SetBool("isRunning",false);
            isRunning = false;
        }

    }



    void FixedUpdate()
    {



// 이동
        if (!isDying)
        {
            // 플레이어가 살아 있는 경우에만 움직임을 처리합니다.
            Move();
        }



// 최대속도 제한
        // 오른쪽으로 이동 (+), 최대 속력을 넘으면
        if (rigid.velocity.x > maxSpeed)
        {
            // 해당 오브젝트의 속력은 maxSpeed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y); 
        }
        // 왼쪽으로 이동 (-) , 최대 속력을 넘으면
        else if (rigid.velocity.x < maxSpeed * (-1))
        {
            // y값은 점프의 영향이므로 0으로 제한을 두면 안됨
            rigid.velocity =  new Vector2(maxSpeed * (-1), rigid.velocity.y); 
        }

    }





// 죽음 함수
    public void OnDie()
    {
        // 죽고 있지 않는다면
        if (!isDying)
        {
            isDying = true;
            canInput = false;
            rigid.velocity = Vector2.zero;

            // Sprite Alpha : 색상 변경 
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            // Sprite Flip Y : 뒤집어지기 
            spriteRenderer.flipY = true;
            // Collider Disable : 콜라이더 끄기 
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            // Die Effect Jump : 아래로 추락 (콜라이더 꺼서 바닥밑으로 추락함)
            rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

            Invoke("Revival",1f);
        }

    }





// 부활 함수
    public void Revival()
    {

        // 원래 색으로 변경
        spriteRenderer.color = new Color(1, 1, 1, 1f);
        // 멀쩡하게 서있게
        spriteRenderer.flipY = false;
        // 콜라이더 켜기
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        MakeInvincible();

        if (lastSpawnPoint == null)
        {
            transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            transform.position = lastSpawnPoint;
        }

        isDying = false;
        canInput = true;
        dead = false;
        // 체력 초기화
        currentHealth = 10;
        // 은신 풀리게
        isHided = false;

        // 모션 초기화
        animator.SetBool("isRunning",false);

        rigid.gravityScale = 8f;

    }





// 속력을 0으로 바꾸는 함수
    public void VelocityZero()
    {

        // 현재 rigidbody의 속도를 가져옵니다.
        Vector2 currentVelocity = rigid.velocity;
        // y축 속도는 그대로 두고 x축 속도를 0으로 설정합니다.
        currentVelocity.x = 0f;
        // 조작된 속도를 rigidbody에 할당합니다.
        rigid.velocity = currentVelocity;

    }





// 이동 & 방향전환
    private void Move()
    {

        // 이동
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h * (maxSpeed / 15f), ForceMode2D.Impulse);

        isRunning = true;
        

        // 움직일 때 방향 바꾸기
        if (Input.GetButton("left") || Input.GetButton("right"))
        {
            animator.SetBool("isRunning", true);
            animator.SetFloat("runSpeed", maxSpeed / 15f);

            if (Input.GetButton("left") && Input.GetButton("right"))
            {
                VelocityZero();
                velocityZero = true;
            }
            else
            {
                if (Input.GetButton("Run"))
                {
                    if (!isFastRunning)
                    {
                        isFastRunning = true;
                        ChangeMaxSpeed(4.5f);
                    }
                }
                else
                {
                    if (isFastRunning)
                    {
                        ChangeMaxSpeed(2/9f);
                        isFastRunning = false;
                    }
                }
                
                if (Input.GetButton("Crouch"))
                {
                    if (!isCrouching)
                    {
                        isCrouching = true;
                        ChangeMaxSpeed(1/9f);
                    }
                }
                else
                {
                    if (isCrouching)
                    {
                        ChangeMaxSpeed(9f);
                        isCrouching = false;
                    }
                }



                velocityZero = false;
            }

            // 왼쪽 화살표를 누를때 왼쪽 보기 / 오른쪽 화살표 누를 때 오른쪽 보기
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
            // 뒤집어졌는지
            isFlipped = Input.GetAxisRaw("Horizontal");
        }

    }





// 대미지 받는 코드
    public void TakeDamage(int damage)
    {

        // 무적 상태가 아닐 때만 데미지를 받음
        if (!isInvincible)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                OnDie();
            }
        }

    }





// 무적 함수
    public void MakeInvincible()
    {

        isInvincible = true;
        // 일정 시간이 지난 후 무적 상태 해제
        Invoke("DisableInvincibility", invincibleDuration);

    }





// 무적 해제 함수
    private void DisableInvincibility()
    {

        isInvincible = false;

    }





// 마지막 스폰포인트로 이동하는 함수
    public void LastPoint()
    {

        lastSpawnPoint = PlayerRoomManager.Instance.GetLastTouchedSpawnPoint();
        player.transform.position = lastSpawnPoint;

    }





// 플레이어 오브젝트와 자식 오브젝트 콜라이더 다 끄는 함수
    public void EnableAllBoxColliders(GameObject obj, bool turnoff)
    {

        // 현재 게임 오브젝트의 모든 BoxCollider2D 컴포넌트를 가져와 비활성화합니다.
        BoxCollider2D[] colliders = obj.GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D collider in colliders)
        {
            collider.enabled = turnoff;
        }

        // 모든 자식 오브젝트들을 순회하며 모든 BoxCollider2D를 비활성화합니다.
        foreach (Transform child in obj.transform)
        {
            EnableAllBoxColliders(child.gameObject,turnoff);
        }

    }





// 최대속도 조절하는 함수
    public void ChangeMaxSpeed(float amount)
    {

        maxSpeed = maxSpeed * amount;

    }





// 소리범위 켜는 함수
    private void ActivateSoundRange()
    {

        soundRange.transform.localScale = new Vector3 (soundAmount, 3, 5);
        soundRange.SetActive(true);

    }





// 소리범위 끄는 함수
    public void DeactivateSoundRange()
    {

        soundRange.SetActive(false);

    }





// 은신 함수
    public void Hide()
    {

        VelocityZero();
        spriteRenderer.color = new Color(1,1,1,0.5f);
        isHided = true;
        animator.SetBool("isRunning",false);

    }





// 은신 해제 함수
    public void GetOutOfHiding()
    {

        VelocityZero();
        spriteRenderer.color = new Color(1,1,1,1f);
        isHided = false;

    }





// 뒤집기 함수
    public void PlayerFlipX(bool flip)
    {

        spriteRenderer.flipX = flip;

    }




// 플레이어의 체력을 저장하는 함수
    public void SavePlayerData(Vector3 position, int health)
    {

        // 플레이어의 체력을 저장
        PlayerPrefs.SetInt("PlayerHealth", health);
        lastSpawnPoint = PlayerRoomManager.Instance.GetLastTouchedSpawnPoint();
        // 변경 사항을 저장
        PlayerPrefs.Save();

        // 디버그 로그로 저장된 데이터 표시
        Debug.Log("Player data saved: Health = " + currentHealth + "\n" +
                  "Last SavePoint : " + lastSpawnPoint);

    }





// 저장된 플레이어 데이터를 불러오는 함수
    public void LoadPlayerData()
    {

        // 플레이어의 체력을 불러옵니다.
        currentHealth = PlayerPrefs.GetInt("PlayerHealth");
        // 플레이어 오브젝트를 찾아 위치와 체력을 설정합니다.
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            LastPoint();
        }

        // 디버그 로그로 불러온 데이터 표시
        Debug.Log("Player data loaded: Health=" + currentHealth + "\n" +
                  "Last SavePoint :" + lastSpawnPoint);

    }





//암살 함수
    void Kill()
    {

        if (soundCheckCode.canKill)
        {
            if (transform.position.x > newEnemy.transform.position.x && !newEnemyCode.isFacingRight)
            {
                newEnemy.SetActive(false);
                Debug.Log("암살");
            }

            else if (transform.position.x < newEnemy.transform.position.x && newEnemyCode.isFacingRight)
            {
                newEnemy.SetActive(false);
                Debug.Log("암살");
            }
        }

    }

}