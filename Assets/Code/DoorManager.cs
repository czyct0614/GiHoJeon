using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{

    private Transform player;
    private Vector3 originalPosition;
    UpDoorMove updoormove;
    DownDoorMove downdoormove;
    
    private float triggerDistance = 5f;

    void Start()
    {

        originalPosition = transform.position;

        player = GameObject.FindWithTag("Player").transform;

        GameObject updoorObject = GameObject.FindWithTag("updoor");
        updoormove = updoorObject.GetComponent<UpDoorMove>();

        GameObject downdoorObject = GameObject.FindWithTag("downdoor");
        downdoormove = downdoorObject.GetComponent<DownDoorMove>();
      
    }





    void Update()
    {

        float distance = Vector3.Distance(transform.position,player.position);

        if (distance < triggerDistance && !updoormove.isgoingup)
        {
            StartCoroutine(updoormove.Up());
        }



        if (distance < triggerDistance && !downdoormove.isgoingdown)
        {
            StartCoroutine(downdoormove.Down());
        }

    }

}