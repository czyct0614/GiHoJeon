using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenCode : MonoBehaviour
{

    private GameObject player;
    private GameObject soundCheck;
    private SpriteRenderer spriteRenderer;

    private NewEnemyCode newEnemyCode;
    private SoundCheckCode soundCheckCode;

    private float distance;
    private float interactDistance = 5f;

    public bool ringing = false;

    public bool hacked;
    public float sirenHackingDuration;
    private bool isHackingActivate;

    public GameObject hackedPrefab;

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        soundCheck = GameObject.FindGameObjectWithTag("SoundCheck");
        spriteRenderer = GetComponent<SpriteRenderer>();
        newEnemyCode = Script.Find<NewEnemyCode>("NewEnemy");
        soundCheckCode = Script.Find<SoundCheckCode>("SoundCheck");
        hacked = false;
        isHackingActivate = false;

    }





    void Update()
    {

        //distance = Vector3.Distance(transform.position, player.transform.position);

        if (/*distance < interactDistance &&*/ hacked && !isHackingActivate)
        {
            StartCoroutine(Ring());
            StartCoroutine(ResetAfterDelay());
        }
        
    }





    private IEnumerator Ring()
    {

        ringing = true;
        soundCheck.transform.localScale = new Vector3(10, 1, 1);

        Debug.Log("사이렌 작동됨");

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.25f);

            spriteRenderer.color = new Color(0.5f, 0, 0, 1f);

            yield return new WaitForSeconds(0.25f);

            spriteRenderer.color = new Color(1, 0, 0, 1f);

            newEnemyCode.isHeared = true;
            soundCheckCode.lastPlayerPoint = new Vector2(transform.position.x, 0);
        }

        newEnemyCode.isHeared = false;
        soundCheck.transform.localScale = new Vector3(20, 1, 1);
        ringing = false;

    }





    private IEnumerator ResetAfterDelay()
    {

        isHackingActivate = true;

        GameObject hackedObject = Instantiate(hackedPrefab, transform.position, transform.rotation);

        yield return new WaitForSeconds(sirenHackingDuration);

        hacked = false;

        isHackingActivate = false;

    }
    
}
