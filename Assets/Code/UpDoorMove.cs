using UnityEngine;
using System.Collections;

public class UpDoorMove : MonoBehaviour
{

    // 플레이어의 Transform
    private Transform player;

    private Vector3 originalPosition;

    // 플레이어와의 거리 임계값
    public float triggerDistance = 5f;
    // 이동할 Y축 거리
    public float moveAmount = 3f;
    // 이동하는 데 걸리는 거리
    private float moveDuration = 0.5f;
    // 움직이는 시간
    public float stayOpenDuration;

    public bool isGoingUp = false;
    public bool isBackUp = false;

    public bool upDoorOpened;
    

   

    void Start()
    {

        originalPosition = transform.position;

        upDoorOpened = false;
        
    }





    public IEnumerator Up()
    {

        isGoingUp = true;

        // Move the door up
        Vector3 targetPosition = originalPosition + new Vector3(0, moveAmount, 0);
        yield return StartCoroutine(MoveToPosition(transform, targetPosition, moveDuration));

        isGoingUp = false;
        upDoorOpened = true;

    }


    public IEnumerator UpDone()
    {

        isBackUp = true;

        // Move the door down
        upDoorOpened = false;
        yield return StartCoroutine(MoveToPosition(transform, originalPosition, moveDuration));

        isBackUp = false;

    }





    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {

        var currentPos = transform.position;
        var t = 0f;
        
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }

    }

}
