using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCode : MonoBehaviour {
    public RoomBounds roomBounds;

    private GameObject playerObject;

    void Start() {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if (playerObject == null) {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player entered room: " + roomBounds.roomName);
            CameraFollow.Instance.SetCurrentRoom(roomBounds);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (CameraFollow.Instance.GetCurrentRoomName() == roomBounds.roomName) {
                CameraFollow.Instance.ClearCurrentRoom();
            }
        }
    }
}