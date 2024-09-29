using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxHoleScript : MonoBehaviour
{

    private GameObject player;
    private NewPlayerCode playermove;

    public bool hided = false;
    public bool entered = false;
    
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        playermove = player.GetComponent<NewPlayerCode>();

    }





    void Update()
    {

        if (hided)
        {
            entered = true;
        }



        if (!hided && Input.GetButtonDown("Interact") && entered)
        {
            playermove.DeactivateSoundRange();
            playermove.EnableAllBoxColliders(player, false);
            player.gameObject.SetActive(false);
            hided = true;
        }

        else if (hided && Input.GetButtonDown("Interact") && entered)
        {
            player.gameObject.SetActive(true);
            playermove.EnableAllBoxColliders(player, true);
            hided = false;
        }

    }





    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            entered = true;
        }

    }





    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            entered = false;
        }
        
    }

}
