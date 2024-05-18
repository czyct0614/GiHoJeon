using UnityEngine;

public class SceneStartPoint : MonoBehaviour
{
    public string savedMap;
    void Start(){
        DontDestroyOnLoad(gameObject);
        if(savedMap==null){
        GM.SaveCurrentMap("SampleScene");
        }
    }
    void Update()
    {
        SaveMapName();
        savedMap = GM.LoadCurrentMap();
    }

    void SaveMapName()
    {
        
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentSceneName != "StartScene")
        {
            GM.SaveCurrentMap(currentSceneName);
        }
    }
}