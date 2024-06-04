using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBar : MonoBehaviour
{
   public Transform CameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        CameraTransform=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        // 플레이어의 위치를 기준으로 UI의 위치를 조정
        transform.position = new Vector3(CameraTransform.position.x, CameraTransform.position.y-16.8f,CameraTransform.position.z+10f);
    }
}

