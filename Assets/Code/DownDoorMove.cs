using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownDoorMove : MonoBehaviour
{

    // 플레이어와의 거리 임계값
    public float triggerDistance = 5f;
    // 이동할 Y축 거리
    public float moveAmount = 3f;
    // 이동하는 데 걸리는 시간
    private float moveDuration = 0.5f;
    // 열린 상태로 유지되는 시간
    public float stayOpenDuration;

    // 플레이어의 Transform
    private Transform player;
    private Vector3 originalPosition;

    public bool isGoingDown = false;
    public bool isBackDown = false;

    public bool downDoorOpened;
    

    void Start()
    {

        originalPosition = transform.position;
        downDoorOpened = false;
        
    }





    public IEnumerator Down()
    {

        isGoingDown = true;

        // Move the door up
        Vector3 targetPosition = originalPosition + new Vector3(0, moveAmount, 0);
        yield return StartCoroutine(MoveToPosition(transform, targetPosition, moveDuration));

          
        isGoingDown  = false;
        downDoorOpened = true;
        
    }

    public IEnumerator DownDone()
    {

        isBackDown = true;
       
        // Move the door down
        downDoorOpened = false;
        yield return StartCoroutine(MoveToPosition(transform, originalPosition, moveDuration));

        isBackDown = false;

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