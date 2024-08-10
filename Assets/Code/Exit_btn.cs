using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit_btn : MonoBehaviour
{

    public void GameExit()
    {

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif

    }

}
