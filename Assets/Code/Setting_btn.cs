using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_btn : MonoBehaviour
{

    public GameObject settingsPanel;

    public void setting()
    {

        // 설정 패널을 활성화하여 보여줍니다.
        settingsPanel.SetActive(true);
        
    }

}
