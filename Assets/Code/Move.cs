using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    private Missile misile;//총알 프리팹
    private Rigidbody2D rigid; //물리이동을 위한 변수 선언
    private SpriteRenderer spriteRenderer; //방향전환을 위한 변수 
    private Animator animator; //애니메이터 조작을 위한 변수 
    private SkillImage skillimage;//스킬 이미지(왼쪽 아래  UI) 변수
    private bool velocityZero = false;
    private bool isInvincible = false; // 무적 상태 여부를 나타내는 변수
    private Collider2D playerCollider;
    Vector2 originPos = new Vector2();//스폰포인트
    private static bool playerExists = false;
    private GameObject player;
    


    [Header("====>총알과 스킬 프리팹<====")]
    public GameObject missilePrefab;//총알 코드 가져오기
    public GameObject skillSelectPrefab;//스킬 선택 코드 가져오기



    [Header("====>모션<====")]
    public bool IsJumping = false;//점프 모션
    private bool IsRunning = false;//달리기 모션
    private bool IsShooting = false;//총쏘는 모션
    private bool canShoot = true;
    private bool isClimbing = false;



    [Header("====>체력<====")]
    public int maxHealth = 10;//최대체력
    public int currentHealth = 10;//현재체력



    [Header("====>이동<====")]
    public float maxSpeed; //최대 속력 변수 
    public float isFlipped;
    public bool isFastRunning;



    [Header("====>점프<====")]
    public float jumpPower;//점프 높이
    private float JumpTime;
    private float CanJumpTime = 0.4f;
    public bool CanJump = true;

    

    [Header("====>대쉬<====")]
    public float dashingPower=9f;//얼마나 많이 움직일지
    public float dashingTime=0.2f;//대쉬 지속시간
    public float dashingCooldown=1f;//대쉬 쿨타임
    private bool canDash=true;//대쉬가능한지
    private bool isDashing;//대쉬하고있는지
    

    
    [Header("====>s점프<====")]
    public bool playerOnPlatform = false;



    [Header("====>죽음/부활<====")]
    public float InvincibleDuration = 0.5f;//부활 무적시간
    public Vector2 lastSpawnPoint;
    public string NowMap;
    public bool Dead=false;
    public bool isDying=false;//죽고 있는지
    public bool canInput = true;



    [Header("====>그외<====")]
    public bool isSkillReady = false;
    public bool isHided = false;
    public float climbSpeed = 10f; // 사다리 오르기 속도
    public float shootCooldown;//장전시간
    [SerializeField] private TrailRenderer tr;//대쉬 잔상 남기기
    [SerializeField] private Image HPbarImage;
    [SerializeField] private Image ChargebarImage;





//시작 함수
    void Awake() {

        NowMap=SceneManager.GetActiveScene().name;

        player = GameObject.FindGameObjectWithTag("Player");

        Time.timeScale = 1f;

        DontDestroyOnLoad(this.gameObject);//맵 바꿔도 안 날아가게

        originPos = transform.position;//게임 시작하면 위치 저장

        rigid = GetComponent<Rigidbody2D>(); //변수 초기화 

        spriteRenderer = GetComponent<SpriteRenderer>(); // 초기화 

        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        ChangeHealthBarAmount();

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



        maxHealth = 10;//최대체력

        currentHealth = 10;//현재체력

        dashingPower=9f;//얼마나 많이 움직일지

        dashingTime=0.2f;//대쉬 지속시간

        dashingCooldown=1f;//대쉬 쿨타임

        InvincibleDuration = 0.5f;//부활무적 시간

        shootCooldown=0.15f;//총 장전시간

        Dead=false;

        canDash=true;//대쉬가능한지

        isDying=false;//죽고 있는지

        canInput = true;

        isInvincible = false; // 무적 상태 여부를 나타내는 변수

        CanJumpTime = 0.4f;

        CanJump = true;

        isSkillReady = false;

        isHided = false;
        
        maxSpeed=15f;


    }



//업데이트 함수
    void Update(){

        if (Input.GetMouseButtonDown(0)) {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Clicked position: " + clickPosition);
        }

        if(NowMap!=SceneManager.GetActiveScene().name)
        {

            if (SceneManager.GetActiveScene().name == "StartScene")
            {

                transform.position = new Vector3(0,-3,-10);

                rigid.gravityScale = 0f;

                isDying=true;

            }
            else
            {
                transform.position = new Vector3(transform.position.x,transform.position.y,0f);

                rigid.gravityScale = 8f;

                isDying=false;
            }
            
            NowMap=SceneManager.GetActiveScene().name;

        }
    


//죽고있을떄 다른 행동 못하게
        if(isDying){
            return;
        }



        lastSpawnPoint = PlayerRoomManager.Instance.GetLastTouchedSpawnPoint();

        ChangeHealthBarAmount();


//스킬 불러오기
        if(skillimage==null){
            skillimage = GameObject.FindGameObjectWithTag("SelectedSkill").GetComponent<SkillImage>();
        }
        isSkillReady=skillimage.isSkillReady;



//주인공 콜라이더 불러오기
        if(playerCollider==null){
            playerCollider = GetComponent<Collider2D>();
        }



//주인공 물리엔진 불러오기
        if(rigid==null){
            rigid = GetComponent<Rigidbody2D>();
        }



//주인공 스프라이트 렌더러 불러오기
        if(spriteRenderer==null){
            spriteRenderer = GetComponent<SpriteRenderer>();
        }



//주인공 애니메이터 불러오기
        if(animator==null){
            animator = GetComponent<Animator>();
        }



//체력바 이미지 불러오기
        if(HPbarImage==null){
            HPbarImage=GameObject.FindGameObjectWithTag("HealthBarImage").GetComponent<Image>();
        }



//차징바 이미지 불러오기
        if(ChargebarImage==null){
            ChargebarImage=GameObject.FindGameObjectWithTag("ChargeBarImage").GetComponent<Image>();
        }



//은신
/*
        if(rigid.velocity==new Vector2(0,0)&&Input.GetButtonDown("Interact")){
            if(isHided==false)
            {
                spriteRenderer.color = new Color(1,1,1,0f);
                rigid.gravityScale=0f;
                EnableAllBoxColliders(this.gameObject,false);
                isInvincible=true;
                isHided=true;
            }
            else if(isHided==true)
            {
                spriteRenderer.color = new Color(1,1,1,1f);
                EnableAllBoxColliders(this.gameObject,true);
                rigid.gravityScale=8f;
                isInvincible=false;
                isHided=false;
            }
            else
            {
                Debug.Log("은신 오류남");
            }
        }
*/


//은신시 아무 행동 못하게
        if(isHided==true){
            return;
        }



//달리면서 총쏠때 달리면서총쏘기 모션 실행
        if(IsRunning && IsShooting){
            animator.SetBool("RunningShoot", true);
        }
        else{
            animator.SetBool("RunningShoot", false);
        }



//캐릭터 위치
        Vector2 PlayerPos = GetComponent<Rigidbody2D>().position;
        


//마우스 위치
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);



//낙사
        if(Dead){
            OnDie();
            //죽고 난 뒤 가속도 초기화하기(다시 죽는것 방지)
            rigid.velocity = new Vector2(rigid.velocity.x , 0);
        }



//대쉬중일때 다른 행동 못하게
        if(isDashing){
            return;
        }



//총알 발사

        if (canShoot && Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1)&!isSkillReady)
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
        /*if(Input.GetButtonDown("Dash") && canDash && !velocityZero){
            animator.SetBool("IsDashing",true);
            animator.SetBool("IsJumping",false);
            StartCoroutine(Dash());
            IsRunning = false;
        }*/



//S점프 코드

        if (Input.GetButton("Stop") && Input.GetButtonDown("Jump") && playerOnPlatform)
        {
            playerCollider.enabled = false;
            CanJump = false;
            // 초 후에 플레이어 콜라이더 다시 활성화
            StartCoroutine(EnableColliderAfterDelay(0.25f));
            StartCoroutine(JumpCoolTime(0.3f));
        }

        Climb();
    }



    void FixedUpdate(){


//대쉬하고 있을 때 딴 행동 못하게 하기
        if(isDashing){
            return;
        }



//은신시 아무 행동 못하게
        if(isHided==true){
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

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 2.5f, LayerMask.GetMask("Platform"));
        //빔의 시작위치, 빔의 방향 , 1:distance , ( 빔에 맞은 오브젝트를 특정 레이어로 한정 지어야할 때 사용 ) // RaycastHit2D : Ray에 닿은 오브젝트 클래스 
    
        if(!isDashing){ 
            
            if(rayHit.collider == null && !isClimbing){
                animator.SetBool("IsJumping",true);
            
            }
        }

        if(isDashing){
            animator.SetBool("IsJumping",false);
            animator.SetBool("IsRunning",false);
            animator.SetBool("IsDashing",true);
        }


        if(rayHit.collider != null){ //빔을 맞은 오브젝트가 있을때

            if(rayHit.distance < 6f)
                

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





//부활 함수
    public void Revival(){

        //원래 색으로 변경
        spriteRenderer.color = new Color(1,1,1,1f);

        //멀쩡하게 서있게
        spriteRenderer.flipY = false;

        //콜라이더 켜기
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        MakeInvincible();

        if(lastSpawnPoint==null){
            transform.position=new Vector3(0,0,0);
        }
        else{
            transform.position=lastSpawnPoint;
        }

        //죽을수 있게
        isDying=false;

        canInput = true;

        Dead = false;

        //체력 초기화
        currentHealth = 10;

        //대쉬할수 있게
        canDash = true;

        //은신 풀리게
        isHided=false;

        //모션 초기화
        animator.SetBool("IsJumping",false);

        animator.SetBool("IsRunning",false);

        animator.SetBool("IsDashing",false);

        tr.emitting=false;

        rigid.gravityScale=8f;

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

        IsRunning = true;

        rigid.AddForce(Vector2.right * h*(maxSpeed/15f), ForceMode2D.Impulse);

        //움직일때 방향 바꾸기
        if(Input.GetButton("left") || Input.GetButton("right")){

            if(!IsJumping && !isDashing)
            {
                animator.SetBool("IsRunning",true);
                animator.SetFloat("RunSpeed", maxSpeed/15f);
            }

            if(Input.GetButton("left") && Input.GetButton("right"))
            {
                
                VelocityZero();
                velocityZero = true;

            }
            else{
                if(Input.GetButton("Run")){
                    if(!isFastRunning){
                        isFastRunning=true;
                        ChangeMaxSpeed(1.5f);
                    }
                }
                else
                {
                    if(isFastRunning){
                        ChangeMaxSpeed(2/3f);
                        isFastRunning=false;
                    }
                }
                velocityZero = false;
            }

            //왼쪽 화살표를 누를때 왼쪽 보기/오른쪽 화살표 누를 때 오른쪽 보기
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

            //뒤집어졌는지
            isFlipped=Input.GetAxisRaw("Horizontal");
        }
    }





//대미지 받는 코드
    public void TakeDamage(int damage){   

        if (!isInvincible){ // 무적 상태가 아닐 때만 데미지를 받음

            currentHealth -= damage;

            //Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

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

        animator.SetBool("IsRunning",false);

        animator.SetBool("IsJumping",false);

        isInvincible = true;

        //대쉬중에 재대쉬 못하게 막음
        canDash=false;

        //대쉬하고 있을때 딴 코드 실행 안되게
        isDashing=true;

        //원래 중력 저장
        float originalGravity=8f;

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

        if(HPbarImage != null){

            HPbarImage.fillAmount = (float)currentHealth/maxHealth;

        }

    }





// //차지게이지 함수
//     public void ChangeChargeBarAmount(float min,float max){ //* Charge 게이지 변경 

//         if(ChargebarImage != null){

//         ChargebarImage.fillAmount = min/max;

//         }

//     }





// 지연 후 플레이어 콜라이더 활성화하는 코루틴
    private IEnumerator EnableColliderAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);

        // 플레이어 콜라이더 활성화
        playerCollider.enabled = true;

    }





//점프 쿨타임 함수
    private IEnumerator JumpCoolTime(float delay)
    {

        yield return new WaitForSeconds(delay);

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





// 플레이어의 체력을 저장하는 함수
    public void SavePlayerData(Vector3 position, int health)
    {
        // 플레이어의 체력을 저장
        PlayerPrefs.SetInt("PlayerHealth", health);

        lastSpawnPoint = PlayerRoomManager.Instance.GetLastTouchedSpawnPoint();

        // 변경 사항을 저장
        PlayerPrefs.Save();

        // 디버그 로그로 저장된 데이터 표시
        Debug.Log("Player data saved: Health=" + currentHealth + "\n" +
                  "Last SavePoint :" + lastSpawnPoint);

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

            LastPoint();

        }

        // 디버그 로그로 불러온 데이터 표시
        Debug.Log("Player data loaded: Health=" + currentHealth + "\n" +
                  "Last SavePoint :" + lastSpawnPoint);

    }





//사다리 탈때 중력 0되는 코드
    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Ladder"))

        {

            isClimbing = true;

            rigid.gravityScale = 0f; // 중력을 0으로 설정

            rigid.velocity = new Vector2(rigid.velocity.x, 0f); // 현재 속도를 초기화

            animator.SetBool("IsClimbing", true); // 애니메이션 설정

            animator.SetBool("IsJumping", false);

            animator.SetBool("IsRunning", false);

        }

    }





//사다리 벗어나면 중력 돌아오는 함수
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Ladder"))
        {

            isClimbing = false;

            rigid.gravityScale = 8f; // 중력을 원래대로 설정

            animator.SetBool("IsClimbing", false); // 애니메이션 해제

        }

    }





//사다리 오르는 함수
    void Climb()
    {

        if (isClimbing)
        {

            float vertical = Input.GetAxis("Vertical"); // 수직 입력값 가져오기
            
            rigid.velocity = new Vector2(rigid.velocity.x, vertical * climbSpeed); // 사다리 오르기 속도 설정

            if (Mathf.Abs(vertical) > 0.1f) // 사다리를 오르거나 내릴 때 애니메이션 설정
            {
                animator.speed = 1; // 애니메이션 재생 속도 정상
            }
            else
            {
                animator.speed = 0; // 애니메이션 정지
            }
        }
        else
        {
            animator.speed = 1; // 사다리를 벗어나면 애니메이션 재생 속도 정상
        }
    }





//마지막 스폰포인트로 이동하는 함수
    public void LastPoint(){
        lastSpawnPoint = PlayerRoomManager.Instance.GetLastTouchedSpawnPoint();
        player.transform.position = lastSpawnPoint;
    }





//플레이어 오브젝트와 자식 오브젝트 콜라이더 다 끄는 함수
    void EnableAllBoxColliders(GameObject obj,bool turnoff)
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





//소리 범위 켜고 늘리고 끄는 함수
    void ChangeSoundRange(){

    }




//최대속도 조절하는 함수
    public void ChangeMaxSpeed(float amount){
        maxSpeed=maxSpeed*amount;
    }
}