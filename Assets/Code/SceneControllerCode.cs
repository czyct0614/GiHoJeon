using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    private static SceneController instance;

    // Player 관련 변수
    private GameObject player;
    private NewPlayerCode playerMove;

    // UI 관련 변수
    private GameObject uiCanvas;  // UI 오브젝트를 참조할 변수

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

        // Start에서 Player와 UI 오브젝트를 찾습니다.
        player = GameObject.FindGameObjectWithTag("Player");
        uiCanvas = GameObject.FindGameObjectWithTag("UI");  // UI 오브젝트는 "UI" 태그로 찾습니다.

        if (player != null)
        {
            playerMove = player.GetComponent<NewPlayerCode>();

            if (playerMove == null)
            {
                Debug.LogError("PlayerMove component not found on player object.");
            }
        }
        else
        {
            Debug.LogError("Player object not found.");
        }

        if (uiCanvas == null)
        {
            Debug.LogError("UI Canvas object not found.");
        }

        HandlePlayerAndUIActivation();

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
            // 플레이어 전체 활성화
            player.SetActive(true);

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

        if (uiCanvas != null)
        {
            // UI 전체 활성화
            uiCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("UI Canvas object not found.");
        }

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // After the new scene is loaded, handle player and UI activation
        HandlePlayerAndUIActivation();

    }

    private void HandlePlayerAndUIActivation()
    {

        if (player != null)
        {
            if (SceneManager.GetActiveScene().name == "StartScene")
            {
                // 플레이어 전체 비활성화
                player.SetActive(false);
            }
            else
            {
                // 플레이어 전체 활성화
                player.SetActive(true);

                var playerMove = player.GetComponent<NewPlayerCode>();

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

        if (uiCanvas != null)
        {
            if (SceneManager.GetActiveScene().name == "StartScene")
            {
                // UI 전체 비활성화
                uiCanvas.SetActive(false);
            }
            else
            {
                // UI 전체 활성화
                uiCanvas.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("UI Canvas object not found.");
        }

    }

}
