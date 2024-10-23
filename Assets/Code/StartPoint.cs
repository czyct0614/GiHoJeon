using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{

    public NewPlayerCode newPlayerCode;
    public SaveManager saveManager;

    void Start()
    {

        // 플레이어를 찾아서 그 위치로 이동시킵니다.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        newPlayerCode = player.GetComponent<NewPlayerCode>();
        saveManager = FindObjectOfType<SaveManager>();
        
        if (player != null)
        {
            player.transform.position = transform.position;
            PlayerRoomManager.Instance.SetLastTouchedSpawnPoint(transform.position);
            newPlayerCode.LastPoint();
            saveManager.SaveGame();
            
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }

    }

}
