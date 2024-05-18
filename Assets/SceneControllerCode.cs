using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;
    private GameObject player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Start에서 Player 오브젝트를 찾습니다.
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player object not found. Make sure your player object has the 'Player' tag.");
        }

        HandlePlayerActivation();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        player.SetActive(true); // 플레이어 전체 활성화
        player.GetComponent<PlayerMove>().enabled = true;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // After the new scene is loaded, handle player activation
    }

    private void HandlePlayerActivation()
    {
        if (player != null)
        {
            if (SceneManager.GetActiveScene().name == "StartScene")
            {
                player.SetActive(false); // 플레이어 전체 비활성화
            }
            else
            {
                player.SetActive(true); // 플레이어 전체 활성화
                player.GetComponent<PlayerMove>().enabled = true;
            }
        }
    }
}