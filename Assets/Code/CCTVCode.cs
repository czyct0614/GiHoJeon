using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVCode : MonoBehaviour
{

    private bool didThisEverChangedDangerRate = false;

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (!didThisEverChangedDangerRate)
            {
                Script.Find<DangerRate>("DangerBar").ChangeDangerRate(1);
                didThisEverChangedDangerRate = true;
            }
        }
        
    }

}
