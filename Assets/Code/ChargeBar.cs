using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBar : MonoBehaviour
{
   public Transform PlayerTransform;
    // Start is called before the first frame update
    void Start()
    {
        PlayerTransform=GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        // 플레이어의 위치를 기준으로 UI의 위치를 조정
        transform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y+3f,PlayerTransform.position.z);
    }
}
