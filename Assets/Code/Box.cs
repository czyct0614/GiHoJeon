using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private GameObject player;
    private bool hideInRight = true;
    private bool entered;
    private NewPlayerCode playerMove;
    private bool hided;
    private PlayerInput playerInput; // PlayerInput 인스턴스

    void Start()
    {
        hided = false;
        entered = false;
        playerMove = Script.Find<NewPlayerCode>("Player");
        player = GameObject.FindGameObjectWithTag("Player");
    }





    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Enable(); // 입력 활성화
    }





    private void OnDisable()
    {
        playerInput.Disable(); // 입력 비활성화
    }





    void Update()
    {

        if (playerInput.Player.Interact.triggered && entered)
        {
            if (!hided)
            {
                
                playerMove.Hide();
                playerMove.PlayerFlipX(hideInRight);
                player.transform.position = new Vector3(transform.position.x+(hideInRight?1:-1),
                                                        player.transform.position.y,
                                                        player.transform.position.z);

                hided = true;
                entered = true;

            }
            else
            {

                playerMove.GetOutOfHiding();
                hided = false;

            }
        }

        if (hided)
        {
            playerMove.VelocityZero();
        }

    }





    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x > transform.position.x)
            {
                hideInRight = true;
            }
            else
            {
                hideInRight = false;
            }

            entered = true;
        }

    }





    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            entered = false;
        }

    }

}
