using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCheckCode : MonoBehaviour
{

    private NewEnemyMove newEnemyMove;
    private GameObject player;
    public Vector2 lastPlayerPoint;

    void Start()
    {

        newEnemyMove = GetComponentInParent<NewEnemyMove>();
        player = GameObject.FindGameObjectWithTag("Player");

    }





    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("SoundRange"))
        {
            newEnemyMove.isHeared = true;
            lastPlayerPoint = new Vector2(player.transform.position.x, 0);
        }

    }





    void OnTriggerExit2D(Collider2D other)
    {

        newEnemyMove.isHeared = false;

    }

}
