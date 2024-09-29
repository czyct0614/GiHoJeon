using UnityEngine;

public class continue_btn : MonoBehaviour
{

    private SceneController sceneController;
    public GameObject player;
    
    void Start()
    {

        sceneController = FindObjectOfType<SceneController>();

    }





    public void continuee()
    {

        string currentMap = GM.LoadCurrentMap();
        GM gm = FindObjectOfType<GM>();

        if (gm != null)
        {
            // 씬 이동 처리
            sceneController.LoadScene("SampleScene");
            sceneController.LoadScene(currentMap);
            player.GetComponent<NewPlayerCode>().LastPoint();
        }

    }
    
}
