using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Scene 매니저 라이브러리 추가

public class Start_btn : MonoBehaviour
{
    public string transferMapName; // 이동할 맵 이름을 설정합니다.
    // Start is called before the first frame update
    public GameObject player;

    private SceneController sceneController;
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart(){
        // 지정한 씬으로 이동합니다.
        GM.SaveCurrentMap(transferMapName);
        sceneController.LoadScene(transferMapName);
        player.GetComponent<PlayerMove>().Revival();

    }
}
