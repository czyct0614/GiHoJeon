using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{

    private NewEnemyMove enemyScript;
    
    private bool attack = false;

    private PlayerMove playerScript;

    void Start()
    {

        enemyScript = GetComponentInParent<NewEnemyMove>();
        playerScript = Script.Find<PlayerMove>("Player");

    }





    void Update()
    {

        if (attack)
        {
            if (playerScript.isHided)
            {
                // 플레이어가 숨겨졌을 때 적의 시야 방향이 플레이어의 방향과 일치해야 함
                if (enemyScript.isFacingRight != playerScript.spriteRenderer.flipX)
                {
                    enemyScript.OnPlayerDetected();
                }
                else
                {
                    attack = false;
                    enemyScript.isPlayerDetected = false;
                }
            }
            else
            {
                enemyScript.OnPlayerDetected();
            }
        }

    }





    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 시야 범위에 들어옴!");

            // 적에게 플레이어를 추적하라고 알림
            attack = true;
        }

    }





    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 시야 범위에서 나감!");

            // 적에게 플레이어 추적을 중지하라고 알림
            attack = false;
            enemyScript.OnPlayerLost();
        }

    }

}
