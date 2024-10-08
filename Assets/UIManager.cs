using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI[] txt;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<txt.Length;i++)
        {
            txt[i].text = KeySetting.keys[(KeyAction) i].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0;i<txt.Length;i++)
        {
            txt[i].text = KeySetting.keys[(KeyAction) i].ToString();
        }
    }
}
