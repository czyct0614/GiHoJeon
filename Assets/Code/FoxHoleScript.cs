using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxHoleScript : MonoBehaviour
{

    private GameObject player;

    public bool hided = false;
    public bool entered = false;
    
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

    }





    void Update()
    {

        if (hided)
        {
            entered = true;
        }



        if (!hided && Input.GetButtonDown("Interact") && entered)
        {
            player.gameObject.SetActive(false);
            BoxCollider2D collider = player.GetComponent<BoxCollider2D>();
            collider.enabled = false;
            hided = true;
        }
        else if (hided && Input.GetButtonDown("Interact") && entered)
        {
            player.gameObject.SetActive(true);
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
