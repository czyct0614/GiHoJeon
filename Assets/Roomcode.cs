using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCode : MonoBehaviour
{
    public RoomBounds roomBounds;
    private GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            CameraFollow.Instance.SetCurrentRoom(roomBounds);
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            if (CameraFollow.Instance.GetCurrentRoomName() == roomBounds.roomName) {
                CameraFollow.Instance.ClearCurrentRoom();
            }
        }
    }
}
