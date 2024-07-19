using UnityEngine;
using System.Collections;

public class UpDoorMove : MonoBehaviour
{
    private Transform player; // 플레이어의 Transform
    public float triggerDistance = 5f; // 플레이어와의 거리 임계값
    public float moveAmount = 3f; // 이동할 Y축 거리
    public float moveDuration = 2f; // 이동하는 데 걸리는 시간
    public float stayOpenDuration = 3f; // 열린 상태로 유지되는 시간

    private Vector3 originalPosition;
    public bool isgoingup = false;

    void Start()
    {
        originalPosition = transform.position;
        
    }



    public IEnumerator Up()
    {
        isgoingup = true;

        // Move the door up
        Vector3 targetPosition = originalPosition + new Vector3(0, moveAmount, 0);
        yield return StartCoroutine(MoveToPosition(transform, targetPosition, moveDuration));

        // Wait for some time
        yield return new WaitForSeconds(stayOpenDuration);

        // Move the door down
        yield return StartCoroutine(MoveToPosition(transform, originalPosition, moveDuration));

        isgoingup = false;
    }

    private IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
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
