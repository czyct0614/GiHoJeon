using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealUI : MonoBehaviour
{

    public Transform CameraTransform;

    void Awake()
    {

        CameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

    }





    void LateUpdate()
    {

        // 플레이어의 위치를 기준으로 UI의 위치를 조정
        transform.position = new Vector3(CameraTransform.position.x, CameraTransform.position.y, CameraTransform.position.z + 10f);

    }

}
