using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRangeCode : MonoBehaviour
{

    public Transform player;

    void Start()
    {
        
        DontDestroyOnLoad(this);

    }



    void Update()
    {
        
        transform.position = new Vector3(player.position.x, player.position.y, player.position.z);

    }

}
