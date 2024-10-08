using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction {Left, Right, Kill, Run, Crouch, Skill, Interact, KEYCOUNT}

public static class KeySetting {public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>();}
public class KeyManager : MonoBehaviour
{  
    KeyCode[] defaultKeys = new KeyCode[] {KeyCode.A, KeyCode.D, KeyCode.Q, KeyCode.LeftShift, KeyCode.LeftControl, KeyCode.E, KeyCode.F};
    private void Awake()
    {

        for (int i = 0;i < (int) KeyAction.KEYCOUNT;i++)
        {

            KeySetting.keys.Add((KeyAction) i, defaultKeys[i]);

        }

    }





    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if (keyEvent.isKey)
        {
            KeySetting.keys[(KeyAction) key] = keyEvent.keyCode;
            key = -1;
        }

    }




    int key = -1;
    public void ChangeKey(int num)
    {
        key=num;
    }

}
