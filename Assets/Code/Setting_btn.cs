using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_btn : MonoBehaviour
{
    public GameObject settingsPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setting(){
        // 설정 패널을 활성화하여 보여줍니다.
        settingsPanel.SetActive(true);
    } 
}
