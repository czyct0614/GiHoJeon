using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Scene 매니저 라이브러리 추가

public class StartGame : MonoBehaviour
{
    public string transferMapName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Next"))
        {
            // 지정한 씬으로 이동합니다.
            
            SceneManager.LoadScene("SampleScene");

        }
    }

        
}
