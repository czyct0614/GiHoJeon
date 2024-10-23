using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButtonCode : MonoBehaviour
{
    private SceneController sceneController;
    private SaveManager saveManager;

    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        saveManager = FindObjectOfType<SaveManager>();
    }

    public void ContinueGame()
    {
        // 세이브된 데이터를 불러옴
        GameData data = saveManager.LoadGame();

        if (data != null)
        {
            // 데이터가 있으면 해당 맵을 로드
            sceneController.LoadScene(data.playerMapName);
        }
        else
        {
            Debug.LogError("No saved game data to continue from.");
        }
    }
}
