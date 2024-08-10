using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeScreenCode : MonoBehaviour
{

    private Vector3 cameraCenter;

    private Renderer myRenderer;

    public GameObject SbuttonObject;
    public GameObject SbuttonObject2;
    public GameObject SbuttonObject3;
    public GameObject vButtonObject1;
    public GameObject vButtonObject2;
    public GameObject vButtonObject3;
    public GameObject settingsPanel;
    public GameObject VolumePanel;
 
    void Start()
    {

        // 버튼 비활성화
        vButtonObject1.SetActive(false);
        vButtonObject2.SetActive(false);
        vButtonObject3.SetActive(false);
        VolumePanel.SetActive(false);

    }





    void Update()
    {

        if (Input.GetButtonDown("Escape"))
        {
            VolumePanel.SetActive(false);
            settingsPanel.SetActive(true);
            SbuttonObject.SetActive(true);
            SbuttonObject2.SetActive(true);
            SbuttonObject3.SetActive(true);
        }
        else
        {
            settingsPanel.SetActive(false);
            VolumePanel.SetActive(true);
        }

    }





    public void volumescreencode()
    {

        // Renderer 컴포넌트 가져오기
        myRenderer = GetComponent<Renderer>();

        // 카메라 중심 계산
        Vector3 cameraCenter = Camera.main.transform.position;
        // 스프라이트의 상대 위치 계산
        Vector3 offset = transform.position - cameraCenter;
        // 이동할 위치 계산 (카메라와 동일한 x, 현재 y 좌표, z 좌표)
        Vector3 targetPosition = new Vector3(cameraCenter.x, cameraCenter.y, transform.position.z);

        // 스프라이트 이동 (카메라 중심으로)
        transform.position = targetPosition;
        VolumePanel.SetActive(true);
        
        // 버튼 상태 토글
        vButtonObject1.SetActive(true);
        vButtonObject2.SetActive(true);
        vButtonObject3.SetActive(true);

    }

}
