using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCheckCode : MonoBehaviour
{

    private NewEnemy newEnemy;
    private GameObject player;
    public Vector2 lastPlayerPoint;

    void Start()
    {

        newEnemy = GetComponentInParent<NewEnemy>();
        player = GameObject.FindGameObjectWithTag("Player");

    }





    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("SoundRange"))
        {
            newEnemy.isHeared = true;
            lastPlayerPoint = new Vector2(player.transform.position.x, 0);
        }

    }





    void OnTriggerExit2D(Collider2D other)
    {

        newEnemy.isHeared = false;

    }

}
