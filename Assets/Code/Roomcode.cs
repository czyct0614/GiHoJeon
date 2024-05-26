using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCode : MonoBehaviour
{
    public RoomBounds roomBounds;
    private GameObject playerObject;

    // Start 메서드는 첫 프레임 업데이트 전에 호출됩니다.
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraFollow.Instance.SetCurrentRoom(roomBounds);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CameraFollow.Instance.GetCurrentRoomName() == roomBounds.roomName)
            {
                CameraFollow.Instance.ClearCurrentRoom();
            }
        }
    }
}