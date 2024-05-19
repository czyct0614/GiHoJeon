using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;
    private GameObject player;
    private PlayerMove playerMove;

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
        if (player != null)
        {
            playerMove = player.GetComponent<PlayerMove>();
            if (playerMove == null)
            {
                Debug.LogError("PlayerMove component not found on player object.");
            }
        }
        else
        {
            Debug.LogError("Player object not found.");
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

        // Check if player is null before using it
        if (player != null)
        {
            player.SetActive(true); // 플레이어 전체 활성화
            if (playerMove != null)
            {
                playerMove.enabled = true;
            }
            else
            {
                Debug.LogError("PlayerMove component not found on player object.");
            }
        }
        else
        {
            Debug.LogError("Player object not found.");
        }

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // After the new scene is loaded, handle player activation
        HandlePlayerActivation();
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
                var playerMove = player.GetComponent<PlayerMove>();
                if (playerMove != null)
                {
                    playerMove.enabled = true;
                }
                else
                {
                    Debug.LogError("PlayerMove component not found on player object.");
                }
            }
        }
        else
        {
            Debug.LogError("Player object not found.");
        }
    }
}
