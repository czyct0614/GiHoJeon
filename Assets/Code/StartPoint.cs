using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    PlayerMove move;
    void Start()
    {
        // 플레이어를 찾아서 그 위치로 이동시킵니다.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        move=player.GetComponent<PlayerMove>();
        if (player != null)
        {
            player.transform.position = transform.position;
            PlayerRoomManager.Instance.SetLastTouchedSpawnPoint(transform.position);
            move.LastPoint();
            move.Revival();
            
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
