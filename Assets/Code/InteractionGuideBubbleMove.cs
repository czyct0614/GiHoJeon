using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionGuideBubbleMove : MonoBehaviour
{
    private Vector2 startPos;
    public float floatHeight = 0.5f;
    public float floatSpeed = 3f;

    void Start()
    {

        startPos = transform.position;

    }





    void Update()
    {

        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = new Vector2(transform.position.x, newY);

    }

}
