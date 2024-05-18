using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChargingCoolTime : MonoBehaviour
{
    public Transform cameraTransform; // 카메라의 Transform
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);//맵 바꿔도 안 날아가게
    
    }

    void LateUpdate()
    {
        // 플레이어의 위치를 기준으로 UI의 위치를 조정
        transform.position = new Vector3(cameraTransform.position.x - 14.63f, cameraTransform.position.y - 6.88f,cameraTransform.position.z+10f);

    }
}
