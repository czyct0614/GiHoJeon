using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TransferMap : MonoBehaviour
{
    // 이동할 맵 이름을 public으로 설정하여 Inspector에서 설정 가능하게 합니다.
    public string targetMapName;

    // 박스 콜라이더에 닿는 순간 이벤트 발생
    private void OnTriggerEnter2D(Collider2D collision)
    {

        // 충돌한 오브젝트의 이름이 "Player"인 경우
        if (collision.gameObject.name == "Player")
        {

            // 지정한 맵으로 이동합니다.
            LoadTargetStage(targetMapName);

        }

    }





    // 지정한 맵 이름으로 스테이지를 이동합니다.
    public void LoadTargetStage(string mapName)
    {

        // SceneManager를 이용해 해당 맵 이름으로 씬을 로드합니다.
        if (SceneExists(mapName))
        {

            SceneLoader.Instance.LoadtheScene(mapName);
        }
        else
        {

            Debug.LogWarning("해당 맵이 존재하지 않습니다: " + mapName);

        }

    }





    // 맵 이름이 유효한지 확인하는 메서드
    public bool SceneExists(string sceneName)
    {

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {

            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = Path.GetFileNameWithoutExtension(scenePath);

            if (sceneNameFromPath == sceneName)
            {

                return true;

            }

        }
        return false;

    }

}
