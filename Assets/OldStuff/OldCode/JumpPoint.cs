/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPoint : MonoBehaviour
{

    NewPlayerCode playerMove;
    Rigidbody2D rigid; //물리이동을 위한 변수 선언

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerMove = player.GetComponent<NewPlayerCode>();
        rigid = player.GetComponent<Rigidbody2D>(); // rigid 변수 초기화
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FlyingPlatform"))
        {
            playerMove.playerOnPlatform = true;
        }

        

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FlyingPlatform"))
        {
            playerMove.playerOnPlatform = false;
        }
    }
}
*/