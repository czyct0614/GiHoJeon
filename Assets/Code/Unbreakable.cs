using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Unbreakable : MonoBehaviour
{

    void Start()
    {

        //다른 맵에서 없어지지 않게 해줌
        DontDestroyOnLoad(this.gameObject);

    }

    void Update()
    {

        // 현재 씬의 이름을 가져오기
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "StartScene")
        {
            Destroy(gameObject);
        }

    }
    
}
