using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Scene 매니저 라이브러리 추가

public class BTM_btn : MonoBehaviour
{
    public string transferMapName; // 이동할 맵 이름을 설정합니다.

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BTM(){

        // 지정한 씬으로 이동합니다.
        SceneManager.LoadScene(transferMapName);

    }
}
