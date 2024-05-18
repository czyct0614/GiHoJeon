using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance;
    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SceneLoader>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    [SerializeField]
    private CanvasGroup sceneLoaderCanvasGroup;
    [SerializeField]
    private Image progressBar;

    private string loadSceneName;

    public Transform CameraTransform;

    public GameObject maincamera;

    public static SceneLoader Create()
    {
        var SceneLoaderPrefab = Resources.Load<SceneLoader>("SceneLoader");
        return Instantiate(SceneLoaderPrefab);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            maincamera = GameObject.FindWithTag("MainCamera");
            if (maincamera != null)
            {
                CameraTransform = maincamera.transform;
                UpdateLoaderPosition();
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        if (CameraTransform != null)
        {
            CameraTransform = maincamera.transform;
            UpdateLoaderPosition();
        }
        else
        {
            maincamera = GameObject.FindWithTag("MainCamera");
            if (maincamera != null)
            {
                CameraTransform = maincamera.transform;
                UpdateLoaderPosition();
            }
        }
    }

    private void UpdateLoaderPosition()
    {
        transform.position = new Vector3(CameraTransform.position.x, CameraTransform.position.y, CameraTransform.position.z + 10f);
    }

    public void LoadtheScene(string sceneName)
    {
        if (gameObject.activeSelf)
            return;

        gameObject.SetActive(true);
        SceneManager.sceneLoaded += LoadSceneEnd;
        loadSceneName = sceneName;
        StartCoroutine(Load(sceneName));
    }

    private IEnumerator Load(string sceneName)
    {
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        float minimumLoadingTime = 3f; // 최소 로딩 시간 3초
        float startTime = Time.realtimeSinceStartup;

        while (!op.isDone)
        {
            yield return null;

            if (Time.realtimeSinceStartup - startTime < minimumLoadingTime)
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0f, 0.9f, timer / minimumLoadingTime);
            }
            else
            {
                if (op.progress >= 0.9f)
                {
                    progressBar.fillAmount = 1f;
                    op.allowSceneActivation = true;
                    yield break;
                }
                else
                {
                    progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, (op.progress - 0.9f) / 0.1f);
                }
            }
        }
    }

    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= LoadSceneEnd;
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;

        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 2f;
            sceneLoaderCanvasGroup.alpha = Mathf.Lerp(isFadeIn ? 0 : 1, isFadeIn ? 1 : 0, timer);
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}