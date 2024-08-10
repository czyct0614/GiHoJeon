using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{

    private const string MAP_KEY = "CurrentMap";
    private static GM instance;

    private Camera mainCamera;

    public static GM Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GM>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(GM).Name;
                    instance = obj.AddComponent<GM>();
                }
            }

            return instance;
        }
    }

    void Update()
    {

        if (instance != null && instance != this)
        {
            // 중복된 GM 객체를 파괴합니다.
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

    }





    public void SetMainCamera(Camera camera)
    {

        mainCamera = camera;

    }





    public static void SaveCurrentMap(string mapName)
    {

        PlayerPrefs.SetString(MAP_KEY, mapName);
        PlayerPrefs.Save();

    }





    public static string LoadCurrentMap()
    {

        return PlayerPrefs.GetString(MAP_KEY, "SampleScene");

    }





    public void MoveCameraToTargetScene(string targetSceneName)
    {

        StartCoroutine(MoveCameraToScene(targetSceneName));

    }





    private IEnumerator MoveCameraToScene(string targetSceneName)
    {

        // 씬이 존재하는지 확인합니다.
        if (!SceneExists(targetSceneName))
        {

            Debug.LogWarning("Scene does not exist: " + targetSceneName);
            yield break;

        }

        // 타겟 씬을 로드합니다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
        yield return asyncLoad;

        // 타겟 씬이 완전히 로드될 때까지 대기합니다.
        yield return new WaitUntil(() => SceneManager.GetSceneByName(targetSceneName).isLoaded);

        // 카메라가 올바르게 설정되어 있는지 확인합니다.
        if (mainCamera != null)
        {
            // 카메라를 새로운 씬으로 이동시킵니다.
            SceneManager.MoveGameObjectToScene(mainCamera.gameObject, SceneManager.GetSceneByName(targetSceneName));

            // 카메라의 위치와 활성화 상태를 로그로 출력합니다.
            Debug.Log("Camera moved to target scene: " + targetSceneName);
            Debug.Log("Camera position: " + mainCamera.transform.position);
            Debug.Log("Camera active: " + mainCamera.gameObject.activeSelf);
        }
        else
        {
            Debug.LogWarning("Main camera is not set.");
        }

        // SampleScene을 언로드합니다.
        SceneManager.UnloadSceneAsync("SampleScene");

    }





    // 씬이 존재하는지 확인하는 메서드
    private bool SceneExists(string sceneName)
    {

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneNameFromPath == sceneName)
            {
                return true;
            }
        }
        
        return false;

    }
    
}
