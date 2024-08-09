using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxHoleScript : MonoBehaviour
{
    private GameObject player;
    public bool Hided = false;
    public bool entered = false;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Hided){
            entered = true;
        }
        if(!Hided && Input.GetButtonDown("Interact") && entered){
            player.gameObject.SetActive(false);
            BoxCollider2D collider = player.GetComponent<BoxCollider2D>();
            collider.enabled = false;
            Hided = true;
        }
        else if(Hided && Input.GetButtonDown("Interact") && entered){
            player.gameObject.SetActive(true);
            Hided = false;
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
