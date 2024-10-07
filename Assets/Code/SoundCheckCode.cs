using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCheckCode : MonoBehaviour
{

    private NewEnemyCode newEnemyCode;
    private GameObject player;
    public Vector2 lastPlayerPoint;
    private PlayerSoundRange playerScript;
    private DangerRate dangerBarScript;

    public bool canKill = false;

    void Start()
    {

        newEnemyCode = GetComponentInParent<NewEnemyCode>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerSoundRange>();
        dangerBarScript = Script.Find<DangerRate>("DangerBar");

    }





    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("SoundRange") && playerScript.soundAmount >= 7 - dangerBarScript.CheckDangerRate())
        {
            Debug.Log("소리 감지");
            newEnemyCode.isHeared = true;
            lastPlayerPoint = new Vector2(player.transform.position.x, transform.position.y);
        }



        if (other.CompareTag("Player"))
        {
            canKill = true;
            Debug.Log("캔킬");
        }

    }





    void OnTriggerExit2D(Collider2D other)
    {

        newEnemyCode.isHeared = false;
        canKill = false;

    }


    private void Update()
    {
        // DangerRate 스크립트에서 현재 위험도 가져오기
        float currentDangerRate = dangerBarScript.CheckDangerRate();

        // 위험도에 비례하여 사운드체크 오브젝트의 가로 길이 조절
        Vector3 newScale = transform.localScale;
        // 위험도에 따라 1씩 증가
        newScale.x = 20f + (currentDangerRate * 1f);
        transform.localScale = newScale;

        Debug.Log("사운드체크 오브젝트 가로 길이: " + newScale.x);
    }

}
