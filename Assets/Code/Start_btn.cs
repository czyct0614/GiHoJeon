using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Scene 매니저 라이브러리 추가
using UnityEngine.SceneManagement;

public class Start_btn : MonoBehaviour
{

    // 이동할 맵 이름을 설정합니다.
    public string transferMapName;

    public GameObject player;

    private SceneController sceneController;

    void Start()
    {

        sceneController = FindObjectOfType<SceneController>();

    }





    public void GameStart()
    {

        // 지정한 씬으로 이동합니다.
        GM.SaveCurrentMap(transferMapName);
        sceneController.LoadScene(transferMapName);
        
        player.GetComponent<NewPlayerCode>().Revival();

    }

}
