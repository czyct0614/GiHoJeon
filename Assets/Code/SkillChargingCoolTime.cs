using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChargingCoolTime : MonoBehaviour
{
    public Transform CameraTransform; // 카메라의 Transform
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);//맵 바꿔도 안 날아가게
        CameraTransform=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    
    }

    void LateUpdate()
    {
        // 플레이어의 위치를 기준으로 UI의 위치를 조정
        transform.position = new Vector3(CameraTransform.position.x - 14.63f, CameraTransform.position.y - 6.88f,CameraTransform.position.z+10f);

    }
}
