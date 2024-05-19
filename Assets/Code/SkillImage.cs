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
    private SpriteRenderer spriteRenderer;
    public GameObject projectilePrefab;

    // 발사 위치와 충전 관련 변수
    public bool doCharge=false;
    public float maxChargeTime = 1f;
    public float chargeSpeed = 1f;
    public float minProjectileSize = 0.1f;
    public float maxProjectileSize = 1.5f;
    private float currentChargeTime = 0f;
    private GameObject currentProjectile;

    // 스킬 쿨타임 관련 변수
    public float cooldown = 3f;
    private float leftcooldown = 3f;
    private bool skillReady = true;

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
        transform.position = new Vector3(cameraTransform.position.x - 14.63f, cameraTransform.position.y - 6.88f, cameraTransform.position.z + 10f); // UI 위치 설정
        move.ChangeChargeBarAmount(currentChargeTime, maxChargeTime); // 충전 바 UI 업데이트
        
        // 스킬이 준비되어 있을 때만 스킬을 사용할 수 있도록 합니다.
        if (!skillReady)
        {
            return;
        }

        if(move.isDying)
        {
                move.maxSpeed=10;
                currentChargeTime=0f;
                move.ChangeChargeBarAmount(currentChargeTime, maxChargeTime);
                doCharge=false;
        }

        // 발사 버튼이 눌렸을 때
        if (Input.GetButtonDown("Charge") && skillReady)
        {
            doCharge=true;
            currentChargeTime = 0f; // 충전 시간 초기화
            currentProjectile = Instantiate(projectilePrefab, playerTransform.position + Vector3.up * 2f, Quaternion.identity); // 발사체 생성
        }

        // 발사 버튼을 누르고 있는 동안
        if (Input.GetButton("Charge") && currentProjectile != null&doCharge)
        {
            move.maxSpeed=3;
            currentChargeTime += Time.deltaTime * chargeSpeed; // 충전 시간 증가
            float scaleFactor = Mathf.Clamp01(currentChargeTime / maxChargeTime); // 충전 정도 계산
            float projectileSize = Mathf.Lerp(minProjectileSize, maxProjectileSize, scaleFactor); // 발사체 크기 조절
            currentProjectile.transform.localScale = new Vector3(projectileSize, projectileSize, 1f); // 발사체 크기 설정
            currentProjectile.transform.position = playerTransform.position + Vector3.up * 2f; // 발사체 위치 조정

            // 마우스 위치를 바라보도록 발사체 회전
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 PlayerPos = playerTransform.position;
            Vector2 direction = mousePos - PlayerPos;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            currentProjectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // 발사 버튼이 떼졌을 때
        if (Input.GetButtonUp("Charge") && currentProjectile != null)
        {
            doCharge=false;
            // 발사체 방향 설정
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 PlayerPos = playerTransform.position;
            Vector2 direction = (mousePos - PlayerPos).normalized;
            float chargetime=currentChargeTime>maxChargeTime?maxChargeTime:currentChargeTime;
            // 발사 함수 호출
            Launch(direction, chargetime * 2000);

            // 쿨타임 설정 및 스킬 쿨타임 시작
            leftcooldown = cooldown;
            StartCoroutine(SkillCooldown());

            // 플레이어 속도 조정 및 충전 시간 초기화
            move.maxSpeed = 10;
            currentChargeTime = 0f;
            move.ChangeChargeBarAmount(currentChargeTime, maxChargeTime);
        }
    }

    // 스킬 쿨타임을 관리하는 코루틴입니다.
    private IEnumerator SkillCooldown()
    {
        skillReady = false;
        while (leftcooldown > 0.0f)
        {
            leftcooldown -= Time.deltaTime;
            disable.fillAmount = leftcooldown / cooldown;

            int cooltimeSeconds = (int)leftcooldown + 1;
            string cooltimeText = string.Format("{0:D1}", cooltimeSeconds);
            timer.text = cooltimeText;

            yield return new WaitForFixedUpdate();
        }
        timer.text = ""; // 쿨타임이 끝나면 텍스트를 비웁니다.
        skillReady = true; // 스킬이 다시 사용 가능하도록 설정합니다.
    }

    // 발사 함수
    private void Launch(Vector2 Direction, float Speed)
    {
        if (Direction != Vector2.zero)
        {
            Direction.Normalize();
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            currentProjectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        currentProjectile.GetComponent<Rigidbody2D>().AddForce(Direction * Speed);
        currentProjectile = null;
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
        }
    }
}
