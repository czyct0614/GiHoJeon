using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Scene 매니저 라이브러리 추가
using UnityEngine.SceneManagement;
using System.IO;


public class TransferMap : MonoBehaviour
{

    // 이동할 맵 이름을 설정합니다.
    public string transferMapName;

    // 박스 콜라이더에 닿는 순간 이벤트 발생
    private void OnTriggerEnter2D(Collider2D collision)
    {

        // 충돌한 오브젝트의 이름이 "Player"인 경우
        if (collision.gameObject.name == "Player")
        {
            
            // 지정한 씬으로 이동합니다.
            LoadNextStage();
        }

        
    }





    // 다음 스테이지로 이동합니다.
    public void LoadNextStage()
    {

        // 현재 실행 중인 씬의 빌드 인덱스를 가져옵니다.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // 다음 스테이지의 빌드 인덱스를 계산합니다.
        int nextSceneIndex = currentSceneIndex + 1;

        // 만약 다음 스테이지가 존재한다면 해당 씬으로 이동합니다.
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneLoader.Instance.LoadtheScene(GetSceneNameByIndex(nextSceneIndex));
        }
        else
        {
            Debug.LogWarning("다음 스테이지가 없습니다!");
        }

    }





    public string GetSceneNameByIndex(int index)
    {

        if (index >= 0 && index < SceneManager.sceneCountInBuildSettings)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(index);
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            return sceneName;
        }
        else
        {
            Debug.LogError("Invalid scene index: " + index);
            return null;
        }
        
    }

}