using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCheckCode : MonoBehaviour
{

    private NewEnemyCode newEnemyCode;
    private GameObject player;
    public Vector2 lastPlayerPoint;
    private NewPlayerCode playerScript;
    private DangerRate dangerBarScript;

    public bool canKill = false;

    void Start()
    {

        newEnemyCode = GetComponentInParent<NewEnemyCode>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<NewPlayerCode>();
        dangerBarScript = Script.Find<DangerRate>("DangerBar");

    }





    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("SoundRange") && playerScript.soundAmount >= 7 - dangerBarScript.CheckDangerRate())
        {
            Debug.Log("소리 감지");
            newEnemyCode.isHeared = true;
            lastPlayerPoint = new Vector2(player.transform.position.x, 0);
        }



        if (other.CompareTag("Player"))
        {
            canKill = true;
        }

    }





    void OnTriggerExit2D(Collider2D other)
    {

        newEnemyCode.isHeared = false;
        canKill = false;

    }

}
