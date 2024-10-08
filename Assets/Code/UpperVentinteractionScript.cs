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

    void Start()
    {

        player = GameObject.FindWithTag("Player").transform;

    }





    void Update()
    {

        if (entered && Input.GetKeyDown(KeySetting.keys[KeyAction.Interact]))
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
