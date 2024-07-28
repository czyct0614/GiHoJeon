using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperVentinteractionScript : MonoBehaviour
{
    private Transform player;
    public bool Entered = false;
    public Vector3 transformPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Entered && Input.GetButtonDown("Interact")){
            player.position = transformPoint;
        }
    }
    

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Entered = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Entered = false;
        }
    }
}
