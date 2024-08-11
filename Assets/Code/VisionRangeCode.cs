using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{

    private NewEnemy enemyScript;
    
    private bool attack = false;

    void Start()
    {

        enemyScript = GetComponentInParent<NewEnemy>();

    }





    void Update()
    {

        if (attack)
        {
            enemyScript.OnPlayerDetected();
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
