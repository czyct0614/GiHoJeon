using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_Box : MonoBehaviour
{

    private bool InBox = false;

    public PlatformEnemyController PenemyController; // EnemyController의 인스턴스를 저장할 변수

    // Start is called before the first frame update
    void Start()
    {
        //GameObject enemyObject = GameObject.name("Enemy (6)");
        //PenemyController = enemyObject.GetComponent<PlatformEnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(InBox&&PenemyController!=null){
            PenemyController.FollowPlayer();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && PenemyController != null){ // 플레이어와 충돌한 경우
            InBox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.CompareTag("Player") && PenemyController != null){
            InBox = false;
        }
    }
    
}
