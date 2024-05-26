using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMove : MonoBehaviour
{
    PortalManager portalManager;
    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public bool hacked = false;

    public int maxHealth = 3; // 몬스터의 최대 체력
    private int currentHealth; // 현재 체력
    private bool isDead = false; // 몬스터가 죽었는지 여부를 나타내는 변수
    
    private void Start()
    {
        currentHealth = maxHealth; // 몬스터의 체력 초기화
        GameObject portalmanager = GameObject.Find("PortalManager"); // 포탈 찾기
        portalManager=portalmanager.GetComponent<PortalManager>();
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
            portalManager.MonsterDied();// 포탈 매니저에 죽은 몬스터 수를 알림
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
    private void Awake() {
        hacked = false;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        
    }

    void DeActive(){ //오브젝트 끄기 
        gameObject.SetActive(false);
    }
}
