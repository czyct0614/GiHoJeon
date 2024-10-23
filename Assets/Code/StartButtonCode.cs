using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Scene 매니저 라이브러리 추가
using UnityEngine.SceneManagement;

public class StartButtonCode : MonoBehaviour
{

    private SceneController sceneController;
    private SaveManager saveManager;

    public NewPlayerCode newPlayerCode;

    void Start()
    {

        sceneController = FindObjectOfType<SceneController>();
        saveManager = FindObjectOfType<SaveManager>();

    }





    public void GameStart()
    {
        saveManager.StartGame();
        sceneController.LoadScene(newPlayerCode.nowMap);

    }

}
