using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction {Left, Right, Kill, Run, Crouch, Skill, Interact, KEYCOUNT}

public static class KeySetting
{
    public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>();
}

public class KeyManager : MonoBehaviour
{
    public KeyCode[] defaultKeys = new KeyCode[]
    {
        KeyCode.A, KeyCode.D, KeyCode.Q, KeyCode.LeftShift, KeyCode.LeftControl, KeyCode.E, KeyCode.F
    };

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
        {
            KeyAction action = (KeyAction)i;
            if (!KeySetting.keys.ContainsKey(action))
            {
                KeySetting.keys.Add(action, defaultKeys[i]);
            }
            else
            {
                Debug.LogWarning("Key for action " + action + " already exists in the dictionary.");
            }
        }
    }





    public void ChangeKeys(KeyCode[] changeKeys)
    {
        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
        {
            KeyAction action = (KeyAction)i;
            if (!KeySetting.keys.ContainsKey(action))
            {
                KeySetting.keys.Add(action, changeKeys[i]);
            }
            else
            {
                Debug.LogWarning("Key for action " + action + " already exists in the dictionary.");
            }
        }
    }


    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if (keyEvent.isKey && key >= 0)
        {
            KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;
            key = -1;
        }
    }





    int key = -1;

    public void ChangeKey(int num)
    {
        key = num;
    }
}
