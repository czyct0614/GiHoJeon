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

    public GameObject hackedPrefab;

    void Start()
    {

        updoormove = updoorObject.GetComponent<UpDoorMove>();

        
        downdoormove = downdoorObject.GetComponent<DownDoorMove>();

        
        hacked = false;

        isHackingActivate = false;
      
    }





    void Update()
    {

        if (!detected && !hacked && updoormove.upDoorOpened && downdoormove.downDoorOpened && !updoormove.isBackUp && !downdoormove.isBackDown)
        {
           StartCoroutine(updoormove.UpDone());
           StartCoroutine(downdoormove.DownDone());
        }

        

        if (detected && !updoormove.isGoingUp && !hacked)
        {
            StartCoroutine(updoormove.Up());
        }

        

        if (detected && !downdoormove.isGoingDown && !hacked)
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

        GameObject hackedObject = Instantiate(hackedPrefab, transform.position, transform.rotation);

        yield return new WaitForSeconds(doorHackingDuration);

        Destroy(hackedObject);

        hacked = false;

    }

}