using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperVentinteractionScript : MonoBehaviour
{

    private Transform player;

    public Vector3 transformPoint;

    public GameObject Vent;

    public bool entered = false;
    public bool doShow;

    public float changeSpeedRate;
    private PlayerInput playerInput; // PlayerInput 인스턴스

    void Start()
    {

        player = GameObject.FindWithTag("Player").transform;

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

        if (entered && playerInput.Player.Interact.triggered)
        {
            Vent.SetActive(doShow);
            player.position = transformPoint;
            Script.Find<NewPlayerCode>("Player").ChangeMaxSpeed(changeSpeedRate);
        }

    }
    




    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
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
