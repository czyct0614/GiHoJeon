using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCheckCode : MonoBehaviour
{

    private NewEnemy newEnemy;

    void Start()
    {

        newEnemy = GetComponentInParent<NewEnemy>();

    }





    // 충돌한 오브젝트가 트리거 안에 머무를 때 호출되는 메서드
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            newEnemy.isHeared = true;
        }

    }





    void OnTriggerExit2D(Collider2D other)
    {

        newEnemy.isHeared = false;

    }

}
