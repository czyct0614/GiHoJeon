using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMotionCode : MonoBehaviour
{

    private Transform CameraTransform;

    void Awake()
    {

        DontDestroyOnLoad(this.gameObject);

        CameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

    }





    void LateUpdate()
    {

        //UI의 위치를 조정
        transform.position = new Vector3(CameraTransform.position.x, CameraTransform.position.y, CameraTransform.position.z + 10f);

    }

}
