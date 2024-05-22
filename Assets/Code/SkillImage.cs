using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillImage : MonoBehaviour
{
    // 카메라와 플레이어의 Transform을 저장하는 변수
    public Transform cameraTransform;
    public Transform playerTransform;

    // 스프라이트 이미지들과 스킬 발사체 프리팹
    public Sprite newSprite1;
    public Sprite newSprite2;
    public Sprite newSprite3;
    public Sprite newSpriteUltimate;
    private SpriteRenderer spriteRenderer;
    public GameObject projectilePrefab;

    // 발사 위치와 충전 관련 변수
    public bool Skill1DoCharge=false;
    public float Skill1MaxChargeTime = 1f;
    public float Skill1ChargeSpeed = 1f;
    public float Skill1MinProjectileSize = 0.1f;
    public float Skill1MaxProjectileSize = 1.5f;
    private float Skill1CurrentChargeTime = 0f;
    private GameObject Skill1CurrentProjectile;
    private Vector3 Skill1ShootPosition;

    // 스킬1 쿨타임 관련 변수
    public float Skill1CoolDown = 3f;
    private float Skill1LeftCoolDown = 0f;
    private bool Skill1SkillReady = true;

    // 스킬2 쿨타임 관련 변수
    public float Skill2CoolDown = 3f;
    private float Skill2LeftCoolDown = 0f;
    private bool Skill2SkillReady = true;

    // 스킬3 쿨타임 관련 변수
    public float Skill3CoolDown = 3f;
    private float Skill3LeftCoolDown = 0f;
    private bool Skill3SkillReady = true;

    // 궁극기 쿨타임 관련 변수
    public float UltimateSkillCoolDown = 3f;
    private float UltimateSkillLeftCoolDown = 0f;
    private bool UltimateSkillSkillReady = true;

    // 기타 UI 요소들
    public TextMeshProUGUI timer;
    public Image disable;

    //플레이어 죽음시 스킬 초기화
    public PlayerMove move;

    void Awake()
    {
        playerTransform=GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        cameraTransform=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        // SpriteRenderer 컴포넌트를 가져옵니다.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 초기 스프라이트를 설정합니다. (필요에 따라 다르게 설정 가능)
        spriteRenderer.sprite = newSprite1;

        move=ScriptFinder.FindScriptWithTag<PlayerMove>("Player");
    }

    // LateUpdate에서 UI의 위치를 조정하고 스킬 발동 및 쿨타임을 관리합니다.
    void LateUpdate()
    {
        SkillCoolTimeShow();
        transform.position = new Vector3(cameraTransform.position.x - 14.63f, cameraTransform.position.y - 6.88f, cameraTransform.position.z + 10f); // UI 위치 설정
        move.ChangeChargeBarAmount(Skill1CurrentChargeTime, Skill1MaxChargeTime); // 충전 바 UI 업데이트
        
        // 스킬이 준비되어 있을 때만 스킬을 사용할 수 있도록 합니다.
        
        if(spriteRenderer.sprite == newSprite1){
            if (!Skill1SkillReady)
            {
                return;
            }
            if(move.isDying)
            {
                    move.maxSpeed=10;
                    Skill1CurrentChargeTime=0f;
                    move.ChangeChargeBarAmount(Skill1CurrentChargeTime, Skill1MaxChargeTime);
                    Skill1DoCharge=false;
            }

            // 발사 버튼이 눌렸을 때
            if (Input.GetButtonDown("Charge") && Skill1SkillReady)
            {
                Skill1DoCharge=true;
                Skill1CurrentChargeTime = 0f; // 충전 시간 초기화
                Skill1CurrentProjectile = Instantiate(projectilePrefab, playerTransform.position + Vector3.up * 2f, Quaternion.identity); // 발사체 생성
            }

            // 발사 버튼을 누르고 있는 동안
            if (Input.GetButton("Charge") && Skill1CurrentProjectile != null&Skill1DoCharge)
            {
                move.maxSpeed=3;
                Skill1CurrentChargeTime += Time.deltaTime * Skill1ChargeSpeed; // 충전 시간 증가
                float scaleFactor = Mathf.Clamp01(Skill1CurrentChargeTime / Skill1MaxChargeTime); // 충전 정도 계산
                float projectileSize = Mathf.Lerp(Skill1MinProjectileSize, Skill1MaxProjectileSize, scaleFactor); // 발사체 크기 조절
                Skill1CurrentProjectile.transform.localScale = new Vector3(projectileSize, projectileSize, 1f); // 발사체 크기 설정
                Skill1ShootPosition=playerTransform.position + Vector3.up * 2f;
                Skill1CurrentProjectile.transform.position = Skill1ShootPosition; // 발사체 위치 조정

                // 마우스 위치를 바라보도록 발사체 회전
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 SkillPos = new Vector2(Skill1ShootPosition.x,Skill1ShootPosition.y);
                Vector2 direction = mousePos - SkillPos;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Skill1CurrentProjectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            // 발사 버튼이 떼졌을 때
            if (Input.GetButtonUp("Charge") && Skill1CurrentProjectile != null)
            {
                Skill1DoCharge=false;
                // 발사체 방향 설정
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 SkillPos = new Vector2(Skill1ShootPosition.x,Skill1ShootPosition.y);
                Vector2 direction = (mousePos - SkillPos).normalized;
                float chargetime=Skill1CurrentChargeTime>Skill1MaxChargeTime?Skill1MaxChargeTime:Skill1CurrentChargeTime;
                // 발사 함수 호출
                Launch(direction, chargetime * 2000);

                // 쿨타임 설정 및 스킬 쿨타임 시작
                Skill1LeftCoolDown = Skill1CoolDown;
                StartCoroutine(Skill1CoolDownCount());

                // 플레이어 속도 조정 및 충전 시간 초기화
                move.maxSpeed = 10;
                Skill1CurrentChargeTime = 0f;
                move.ChangeChargeBarAmount(Skill1CurrentChargeTime, Skill1MaxChargeTime);
            }
        }
    }

    private void SkillCoolTimeShow(){
        float leftcooldown;
        float cooldown;
        if(spriteRenderer.sprite == newSprite1){
            leftcooldown = Skill1LeftCoolDown;
            cooldown = Skill1CoolDown;
        }
        else if(spriteRenderer.sprite == newSprite2){
            leftcooldown = Skill2LeftCoolDown;
            cooldown = Skill2CoolDown;
        }
        else if(spriteRenderer.sprite == newSprite3){
            leftcooldown = Skill3LeftCoolDown;
            cooldown = Skill3CoolDown;
        }
        else if(spriteRenderer.sprite == newSpriteUltimate){
            leftcooldown = UltimateSkillLeftCoolDown;
            cooldown = UltimateSkillCoolDown;
        }
        else
        {
            leftcooldown=0;
            cooldown=0;
            Debug.Log("스킬 오류남;");
        }
        if(leftcooldown>0.0f){
            disable.fillAmount = leftcooldown / cooldown;
            int cooltimeSeconds = (int)leftcooldown + 1;
            string cooltimeText = string.Format("{0:D1}", cooltimeSeconds);
            timer.text = cooltimeText;
        }
        else{
            disable.fillAmount=0f;
            timer.text = ""; // 쿨타임이 끝나면 텍스트를 비웁니다.
        }
    }

    // 스킬1 쿨타임을 관리하는 코루틴입니다.
    private IEnumerator Skill1CoolDownCount()
    {
        Skill1SkillReady = false;
        while (Skill1LeftCoolDown > 0.0f)
        {
            Skill1LeftCoolDown -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Skill1SkillReady = true; // 스킬이 다시 사용 가능하도록 설정합니다.
    }

    // 스킬2 쿨타임을 관리하는 코루틴입니다.
    private IEnumerator Skill2CoolDownCount()
    {
        Skill2SkillReady = false;
        while (Skill2LeftCoolDown > 0.0f)
        {
            Skill2LeftCoolDown -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        timer.text = ""; // 쿨타임이 끝나면 텍스트를 비웁니다.
        Skill2SkillReady = true; // 스킬이 다시 사용 가능하도록 설정합니다.
    }

    // 스킬3 쿨타임을 관리하는 코루틴입니다.
    private IEnumerator Skill3CoolDownCount()
    {
        Skill3SkillReady = false;
        while (Skill3LeftCoolDown > 0.0f)
        {
            Skill3LeftCoolDown -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        timer.text = ""; // 쿨타임이 끝나면 텍스트를 비웁니다.
        Skill3SkillReady = true; // 스킬이 다시 사용 가능하도록 설정합니다.
    }

    // 궁극기 쿨타임을 관리하는 코루틴입니다.
    private IEnumerator UltimateSkillCoolDownCountS()
    {
        UltimateSkillSkillReady = false;
        while (UltimateSkillLeftCoolDown > 0.0f)
        {
            UltimateSkillLeftCoolDown -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        timer.text = ""; // 쿨타임이 끝나면 텍스트를 비웁니다.
        UltimateSkillSkillReady = true; // 스킬이 다시 사용 가능하도록 설정합니다.
    }

    // 발사 함수
    private void Launch(Vector2 Direction, float Speed)
    {
        if (Direction != Vector2.zero)
        {
            Direction.Normalize();
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            Skill1CurrentProjectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        Skill1CurrentProjectile.GetComponent<Rigidbody2D>().AddForce(Direction * Speed);
        Skill1CurrentProjectile = null;
    }

    // 스프라이트를 변경하는 함수입니다.
    public void ChangeSprite(int SpriteNumber)
    {
        switch (SpriteNumber)
        {
            case 1:
                spriteRenderer.sprite = newSprite1;
                break;
            case 2:
                spriteRenderer.sprite = newSprite2;
                break;
            case 3:
                spriteRenderer.sprite = newSprite3;
                break;
            case 4:
                spriteRenderer.sprite = newSpriteUltimate;
                break;
        }
        Debug.Log(SpriteNumber);
    }
}
