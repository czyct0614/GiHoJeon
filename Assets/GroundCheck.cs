using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private Transform player;
    private Vector3 originalPosition;
    
    private float triggerDistance = 5f;

    
    
    UpDoorMove updoormove;
    DownDoorMove downdoormove;

    // Start is called before the first frame update
    void Start()
    {
      originalPosition = transform.position;
      player = GameObject.FindWithTag("Player").transform;
      GameObject updoorObject = GameObject.FindWithTag("updoor");
      updoormove= updoorObject.GetComponent<UpDoorMove>();
      GameObject downdoorObject = GameObject.FindWithTag("downdoor");
      downdoormove= downdoorObject.GetComponent<DownDoorMove>();
      
    }

    // Update is called once per frame
    void Update()
    {
      float distance = Vector3.Distance(transform.position,player.position);
       if (distance< triggerDistance && !updoormove.isgoingup){
        StartCoroutine(updoormove.Up());
       }
       if (distance< triggerDistance && !downdoormove.isgoingdown){
        StartCoroutine(downdoormove.Down());
       } 
    }
}