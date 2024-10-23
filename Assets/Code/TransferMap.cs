using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TransferMap : MonoBehaviour
{
    // 이동할 맵 이름을 public으로 설정하여 Inspector에서 설정 가능하게 합니다.
    public string targetMapName;
    
    private SceneController sceneController;

    private SaveManager saveManager;

    void Start()
    {

        sceneController = FindObjectOfType<SceneController>();
        saveManager = FindObjectOfType<SaveManager>();

    }




    // 박스 콜라이더에 닿는 순간 이벤트 발생
    private void OnTriggerEnter2D(Collider2D collision)
    {

        // 충돌한 오브젝트의 이름이 "Player"인 경우
        if (collision.gameObject.name == "Player")
        {
            // 지정한 씬으로 이동합니다.
            GM.SaveCurrentMap(targetMapName);
            sceneController.LoadScene(targetMapName);

        }

    }

}
