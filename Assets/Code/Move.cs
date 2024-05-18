

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//앨랠래 속 냉장고
//이동 - A/D
//멈추기 - Left Shift
//가속 - Ctrl
//총 발사 - 마우스 왼쪽 클릭
//대쉬- Space
public class PlayerMove : MonoBehaviour
{
    Missile misile;//총알 프리팹



    public Rigidbody2D rigid; //물리이동을 위한 변수 선언
    SpriteRenderer spriteRenderer; //방향전환을 위한 변수 
    Animator animator; //애니메이터 조작을 위한 변수 



    public GameObject missilePrefab;//총알 코드 가져오기
    public GameObject skillSelectPrefab;//스킬 선택 코드 가져오기
    public GameObject ground;



    public bool IsJumping = false;
    private bool IsRunning = false;
    private bool IsShooting = false;
    private bool canShoot = true;


    public int maxHealth = 10;
    private int currentHealth = 10;



    public float maxSpeed; //최대 속력 변수 
    public float jumpPower;//점프 높이
    public float dashingPower=9f;//얼마나 많이 움직일지
    public float dashingTime=0.2f;//대쉬 지속시간
    public float dashingCooldown=1f;//대쉬 쿨타임
    public float isFlipped;
    public float InvincibleDuration = 0.5f;
    public float shootCooldown;



    public bool Dead=false;
    private bool canDash=true;//대쉬가능한지
    private bool isDashing;//대쉬하고있는지
    public bool isDying=false;//죽고 있는지
    public bool canInput = true;
    private bool isInvincible = false; // 무적 상태 여부를 나타내는 변수
    private float JumpTime;
    private float CanJumpTime = 0.6f;
    public bool CanJump = true;



    Vector2 originPos = new Vector2();//스폰포인트
    
    
    
    public Transform respawnPoint;
    public Transform firePoint;



    private Collider2D playerCollider;
    public bool playerOnPlatform = false;
    

    
    
    [SerializeField] private TrailRenderer tr;//대쉬 잔상 남기기
    [SerializeField] private Image HPbarImage;
    [SerializeField] private Image ChargebarImage;
    private static bool playerExists = false;
    private GameObject player;


//시작 함수
    void Awake() {

        Time.timeScale = 1f;

        DontDestroyOnLoad(this.gameObject);//맵 바꿔도 안 날아가게

        originPos = transform.position;//게임 시작하면 위치 저장

        rigid = GetComponent<Rigidbody2D>(); //변수 초기화 

        spriteRenderer = GetComponent<SpriteRenderer>(); // 초기화 

        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        ChangeHealthBarAmount();

        ChangeChargeBarAmount(0f,1f);

        playerCollider = GetComponent<Collider2D>();

        shootCooldown = 0.1f;

        respawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Transform>();
        
        player = GameObject.FindGameObjectWithTag("Player");

        LoadPlayerData();

        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            player.SetActive(false); // 첫 씬에서 플레이어 비활성화
        }

        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }



//업데이트 함수
    void Update(){
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            player.SetActive(false); // 첫 씬에서 플레이어 비활성화
        }
        
        if(respawnPoint==null){
            respawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Transform>();
        }

        if(playerCollider==null){
            playerCollider = GetComponent<Collider2D>();
        }

        if(rigid==null){
            rigid = GetComponent<Rigidbody2D>();
        }

        if(spriteRenderer==null){
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if(animator==null){
            animator = GetComponent<Animator>();
        }

        if(HPbarImage==null||ChargebarImage==null){
            HPbarImage=GameObject.FindGameObjectWithTag("HealthBarImage").GetComponent<Image>();

            ChargebarImage=GameObject.FindGameObjectWithTag("ChargeBarImage").GetComponent<Image>();
        }

        if(isDying){
            return;
        }

        if(IsRunning && IsShooting){
            animator.SetBool("RunningShoot", true);
        }
        else{
            animator.SetBool("RunningShoot", false);
        }

        Vector2 PlayerPos = GetComponent<Rigidbody2D>().position;//캐릭터 위치(좌표)
        


        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//마우스 위치(좌표)



//대쉬중일때 다른 행동 못하게
        if(isDashing){
            return;
        }



//총알 발사

        if (canShoot && Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1))
        {
            StartCoroutine(ShootWithCooldown());
        }
        else
        {
            animator.SetBool("IsShooting", false);
        }



//스킬창 불러오기
        if (Input.GetMouseButtonDown(1)){

            //마우스 위치로 스킬 선택창 복제하기
            GameObject select = Instantiate(skillSelectPrefab,MousePos,transform.rotation);
        }



//점프
        if(Input.GetButtonDown("Jump") && !animator.GetBool("IsJumping") && canInput && !Input.GetButton("Stop") && CanJump){//스페이스 버튼을 누르면서 점프중이 아닐때
        

            //위로 힘 가하기
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsRunning", false);
            JumpTime = Time.time;  // 현재 시간을 저장
            rigid.AddForce(Vector2.up* jumpPower , ForceMode2D.Impulse);

            //점프 중이라는 신호 보내기
            IsJumping = true;
            IsRunning = false;
            IsShooting = false;
        }

        if(Time.time - JumpTime >= CanJumpTime){
                CanJump = true;
            }

        if(Time.time - JumpTime < CanJumpTime){
                CanJump = false;
            }

 

//서서히 정지 
        if(Input.GetButtonUp("left") || Input.GetButtonUp("right")){

            // normalized : 벡터 크기를 1로 만든 상태 (단위벡터 : 크기가 1인 벡터)
            // 벡터는 방향과 크기를 동시에 가지는데 크기(- : 왼 , + : 오)를 구별하기 위하여 단위벡터(1,-1)로 방향을 알수 있도록 단위벡터를 곱함 
            rigid.velocity = new Vector2( 0f * rigid.velocity.normalized.x , rigid.velocity.y);
        }



//속도 낮을때 멈추기
        if( Mathf.Abs(rigid.velocity.x) < 0.8){ //속도가 0 == 멈춤 

            //isWalking 변수 : false-
            animator.SetBool("IsRunning",false);
            IsRunning = false;
            IsShooting = false;
        }


//대쉬 코드
        if(Input.GetButtonDown("Dash")&canDash){
            animator.SetBool("IsDashing",true);
            animator.SetBool("IsJumping",false);
            StartCoroutine(Dash());
            IsRunning = false;
        }



//낙사
        if(Dead){
            OnDie();
            //죽고 난 뒤 가속도 초기화하기(다시 죽는것 방지)
            rigid.velocity = new Vector2(rigid.velocity.x , 0);
        }

//S점프 코드

        if (Input.GetButton("Stop") && Input.GetButtonDown("Jump") && playerOnPlatform)
        {
            playerCollider.enabled = false;
            CanJump = false;
            Debug.Log("Down");
            // 초 후에 플레이어 콜라이더 다시 활성화
            StartCoroutine(EnableColliderAfterDelay(0.25f));
        }
    }



    void FixedUpdate(){


//대쉬하고 있을 때 딴 행동 못하게 하기
        if(isDashing){
            return;
        }



//이동
        if (!isDying)
        {
                // 플레이어가 살아 있는 경우에만 움직임을 처리합니다.
                Move();
            }



//최대속도 제한
        if(rigid.velocity.x > maxSpeed) {//오른쪽으로 이동 (+) , 최대 속력을 넘으면 

            //해당 오브젝트의 속력은 maxSpeed
            rigid.velocity= new Vector2(maxSpeed, rigid.velocity.y); 
        }
        else if(rigid.velocity.x < maxSpeed*(-1)){ // 왼쪽으로 이동 (-) , 최대 속력을 넘으면

            //y값은 점프의 영향이므로 0으로 제한을 두면 안됨
            rigid.velocity =  new Vector2(maxSpeed*(-1), rigid.velocity.y); 
        }



//점프 모션
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0)); //빔을 쏨(디버그는 게임상에서보이지 않음 ) 시작위치, 어디로 쏠지, 빔의 색 

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 2, LayerMask.GetMask("Platform"));
        //빔의 시작위치, 빔의 방향 , 1:distance , ( 빔에 맞은 오브젝트를 특정 레이어로 한정 지어야할 때 사용 ) // RaycastHit2D : Ray에 닿은 오브젝트 클래스 
    
        //rayHit는 여러개 맞더라도 처음 맞은 오브젝트의 정보만을 저장(?)
        if(!isDashing){ 
            
            if(rayHit.collider == null){
                animator.SetBool("IsJumping",true);
            
            }
        }

        if(isDashing){
            animator.SetBool("IsJumping",false);
            animator.SetBool("IsDashing",true);
        }


        if(rayHit.collider != null){ //빔을 맞은 오브젝트가 있을때  -> 맞지않으면 collider도 생성되지않음 

            if(rayHit.distance < 2.5f)
                

                    animator.SetBool("IsJumping",false); //거리가 0.5보다 작아지면 변경
                    IsJumping = false;
            }
        }





//죽음 함수
    public void OnDie(){ 
        if(!isDying){//죽고 있지 않는다면
            tr.emitting=false;
            isDying=true;
            canInput = false;
            canDash = false;
            rigid.velocity = Vector2.zero;

            //Sprite Alpha : 색상 변경 
            spriteRenderer.color = new Color(1,1,1,0.4f);
        
            //Sprite Flip Y : 뒤집어지기 
            spriteRenderer.flipY = true;

            //Collider Disable : 콜라이더 끄기 
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            //Die Effect Jump : 아래로 추락(콜라이더 꺼서 바닥밑으로 추락함 )
            rigid.AddForce(Vector2.up*5, ForceMode2D.Impulse);

            Invoke("Revival",1f);
        }
    }



//스폰포인트 함수
    public void SetRespwan(Vector3 position){

        respawnPoint.position=position;

    }



//스폰포인트 시작 함수
    public void GoToSpawnPoint(){
        transform.position=respawnPoint.position;
    }



//부활 함수
    public void Revival(){

        //원래 색으로 변경
        spriteRenderer.color = new Color(1,1,1,1f);

        //멀쩡하게 서있게
        spriteRenderer.flipY = false;

        //콜라이더 켜기
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        MakeInvincible();

        //스폰포인트에서 부활
        transform.position=respawnPoint.position;

        //죽을수 있게
        isDying=false;

        canInput = true;

        Dead = false;

        //체력 초기화
        currentHealth = 10;

        //대쉬할수 있게
        canDash = true;

        //모션 초기화
        animator.SetBool("IsJumping",false);

        animator.SetBool("IsRunning",false);

        animator.SetBool("IsDashing",false);

        tr.emitting=false;

        //체력바 초기화
        ChangeHealthBarAmount();

    }



//속력을 0으로 바꾸는 함수
    public void VelocityZero()
{
    // 현재 rigidbody의 속도를 가져옵니다.
    Vector2 currentVelocity = rigid.velocity;

    // y축 속도는 그대로 두고 x축 속도를 0으로 설정합니다.
    currentVelocity.x = 0f;

    // 조작된 속도를 rigidbody에 할당합니다.
    rigid.velocity = currentVelocity;
}



//이동&방향전환
    public void Move(){

        //이동
        float h = Input.GetAxisRaw("Horizontal");

        if(IsJumping){
            Debug.Log("Jump");
        }

        IsRunning = true;

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //움직일때 방향 바꾸기
        if(Input.GetButton("left") || Input.GetButton("right")){

            if(!IsJumping && !isDashing){
            animator.SetBool("IsRunning",true);
        }

        if(Input.GetButton("left") && Input.GetButton("right") && !animator.GetBool("IsJumping")){
            
            VelocityZero();

        }

            //왼쪽 화살표를 누를때 왼쪽 보기/오른쪽 화살표 누를 때 오른쪽 보기
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

            //뒤집어졌는지
            isFlipped=Input.GetAxisRaw("Horizontal");
        }


        if(isDashing){
            animator.SetBool("IsRunning",false);
            animator.SetBool("IsDashing",true);
        }

    }



//대미지 받는 코드
    public void TakeDamage(int damage){   

        if (!isInvincible){ // 무적 상태가 아닐 때만 데미지를 받음

            currentHealth -= damage;

            Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

            ChangeHealthBarAmount();


            if (currentHealth <= 0){

                OnDie();

            }
        }
    }



//무적 함수
    public void MakeInvincible(){

        isInvincible = true;

        Invoke("DisableInvincibility", InvincibleDuration); // 일정 시간이 지난 후 무적 상태 해제

    }



//무적해제함수
    private void DisableInvincibility(){

        isInvincible = false;

    }



//대쉬함수
    private IEnumerator Dash(){

        animator.SetBool("IsDashing",true);

        isInvincible = true;

        //대쉬중에 재대쉬 못하게 막음
        canDash=false;

        //대쉬하고 있을때 딴 코드 실행 안되게
        isDashing=true;

        //원래 중력 저장
        float originalGravity=rigid.gravityScale;

        //앞으로 대쉬하기 위해 중력 0으로 바꿈
        rigid.gravityScale=0f;

        //캐릭터가 보고있는 방향으로 대쉬파워만큼 가속도 주기
        rigid.velocity=new Vector2(isFlipped*transform.localScale.x*dashingPower,0f);

        //자취 남기기
        tr.emitting=true;

        //dashingTime동안 대쉬하기
        yield return new WaitForSeconds(dashingTime);

        //자취 끄기
        tr.emitting=false;

        //원래 중력으로 돌려놓기
        rigid.gravityScale=originalGravity;

        //다시 다른 코드 실행되게
        isDashing=false;
        isInvincible = false;
        rigid.velocity = new Vector2( 0.1f * rigid.velocity.normalized.x , rigid.velocity.y);
        animator.SetBool("IsDashing",false);

        //대쉬 쿨타임동안 대쉬 못하게
        yield return new WaitForSeconds(dashingCooldown);

        //다시 대쉬 가능하게
        canDash=true;

    }   



//체력게이지 함수
    private void ChangeHealthBarAmount(){//* HP 게이지 변경

        HPbarImage.fillAmount = (float)currentHealth/maxHealth;

    }



//차지게이지 함수
    public void ChangeChargeBarAmount(float min,float max){ //* Charge 게이지 변경 

        ChargebarImage.fillAmount = min/max;

    }



// 지연 후 플레이어 콜라이더 활성화하는 코루틴
    private IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 플레이어 콜라이더 활성화
        playerCollider.enabled = true;
        CanJump = true;
    }



//총쏘는 함수
    private IEnumerator ShootWithCooldown()
    {
    // 쿨타임 중인 동안은 총을 쏠 수 없음
    canShoot = false;

    animator.SetBool("IsShooting", true);

    //캐릭터 위치(좌표)
    Vector2 PlayerPos = GetComponent<Rigidbody2D>().position;
        
    //마우스 위치(좌표)
    Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //총알이 나가는 방향
    Vector2 Dir = (MousePos - PlayerPos);
    
    // 총알 발사
    GameObject missile = Instantiate(missilePrefab, transform.position, transform.rotation);
    Missile missileScript = missile.GetComponent<Missile>();
    missileScript.Launch(Dir.normalized, 2000);

    // 쿨타임 대기
    yield return new WaitForSeconds(shootCooldown);

    // 쿨타임 종료 후 다시 총을 쏠 수 있도록 설정
    canShoot = true;

    }

// 플레이어의 위치와 체력을 저장하는 함수
    public void SavePlayerData(Vector3 position, int health)
    {
        // 플레이어의 체력을 저장
        PlayerPrefs.SetInt("PlayerHealth", health);

        // 변경 사항을 저장
        PlayerPrefs.Save();
        // 디버그 로그로 저장된 데이터 표시
        Debug.Log("Player data saved: Health=" + health);
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
            ChangeHealthBarAmount();
        }

        // 디버그 로그로 불러온 데이터 표시
        Debug.Log("Player data loaded: Health=" + currentHealth);
    }

}