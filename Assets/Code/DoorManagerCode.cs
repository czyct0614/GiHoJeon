using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManagerCode : MonoBehaviour
{

    private Transform player;
    public UpDoorMove updoormove;
    public DownDoorMove downdoormove;

    public GameObject updoorObject;
    public GameObject downdoorObject;

    public bool hacked;
    public bool detected;

    //public DoorDetectRangeCode doordetectrange;
    
    

    //해킹 지속시간(인스펙터창에서 바꿀 수 있음)
    public int doorHackingDuration;

    //ResetAfterDelay() 코루틴 한번만 실행되게 하는 변수
    private bool isHackingActivate;

    void Start()
    {

        updoormove = updoorObject.GetComponent<UpDoorMove>();

        
        downdoormove = downdoorObject.GetComponent<DownDoorMove>();

        
        hacked = false;

        isHackingActivate = false;
      
    }





    void Update()
    {

        if (!detected && !hacked && updoormove.upDoorOpened && downdoormove.downDoorOpened)
        {
              
           
           StartCoroutine(updoormove.UpDone());
           StartCoroutine(downdoormove.DownDone());
            
        }

        

        if (detected && !updoormove.isgoingup && !hacked)
        {
            StartCoroutine(updoormove.Up());
            
        }

        

        if (detected && !downdoormove.isgoingdown && !hacked)
        {
            StartCoroutine(downdoormove.Down());
            
        }
           


        if (hacked && !isHackingActivate)
        {
            StartCoroutine(ResetAfterDelay());
        }

    }





    private IEnumerator ResetAfterDelay()
    {

        isHackingActivate = true;

        yield return new WaitForSeconds(doorHackingDuration);

        hacked = false;

    }

}