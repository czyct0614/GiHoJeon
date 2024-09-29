using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    private static SceneController instance;

    private GameObject player;
    private NewPlayerCode playerMove;

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

    }

}
