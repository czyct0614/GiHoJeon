using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBar : MonoBehaviour
{
   public Transform CameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);//맵 바꿔도 안 날아가게
        CameraTransform=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        // 플레이어의 위치를 기준으로 UI의 위치를 조정
        transform.position = new Vector3(CameraTransform.position.x, CameraTransform.position.y-9.5f,CameraTransform.position.z+10f);
    }
}
