using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenCode : MonoBehaviour
{

    private GameObject player;
    private GameObject soundCheck;
    private SpriteRenderer spriteRenderer;

    private NewEnemyMove newEnemyMove;
    private SoundCheckCode soundCheckCode;

    private float distance;
    private float interactDistance = 5f;

    public bool ringing = false;

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        soundCheck = GameObject.FindGameObjectWithTag("SoundCheck");
        spriteRenderer = GetComponent<SpriteRenderer>();
        newEnemyMove = Script.Find<NewEnemyMove>("NewEnemy");
        soundCheckCode = Script.Find<SoundCheckCode>("SoundCheck");

    }





    void Update()
    {

        distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < interactDistance && Input.GetButtonDown("Interact"))
        {
            StartCoroutine(Ring());
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

            newEnemyMove.isHeared = true;
            soundCheckCode.lastPlayerPoint = new Vector2(transform.position.x, 0);
        }

        newEnemyMove.isHeared = false;
        soundCheck.transform.localScale = new Vector3(20, 1, 1);
        ringing = false;

    }
    
}
