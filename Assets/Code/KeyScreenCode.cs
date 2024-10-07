using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScreenCode : MonoBehaviour
{

    private Vector3 cameraCenter;

    private Renderer myRenderer;

    public GameObject SbuttonObject;
    public GameObject SbuttonObject2;
    public GameObject SbuttonObject3;
    public GameObject kButtonObject1;
    public GameObject kButtonObject2;
    public GameObject kButtonObject3;
    public GameObject kButtonObject4;
    public GameObject kButtonObject5;
    public GameObject kButtonObject6;
    public GameObject kButtonObject7;
    public GameObject settingsPanel;
    public GameObject KeyPanel;
 
    void Start()
    {

        // 버튼 비활성화
        kButtonObject1.SetActive(false);
        kButtonObject2.SetActive(false);
        kButtonObject3.SetActive(false);
        kButtonObject4.SetActive(false);
        kButtonObject5.SetActive(false);
        kButtonObject6.SetActive(false);
        kButtonObject7.SetActive(false);
        KeyPanel.SetActive(false);

    }





    void Update()
    {

        if (Input.GetButtonDown("Escape"))
        {
            KeyPanel.SetActive(false);
            settingsPanel.SetActive(true);
            SbuttonObject.SetActive(true);
            SbuttonObject2.SetActive(true);
            SbuttonObject3.SetActive(true);
        }
        else
        {
            settingsPanel.SetActive(false);
            KeyPanel.SetActive(true);
        }

    }





    public void keyscreencode()
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
        KeyPanel.SetActive(true);
        
        // 버튼 상태 토글
        kButtonObject1.SetActive(true);
        kButtonObject2.SetActive(true);
        kButtonObject3.SetActive(true);
        kButtonObject4.SetActive(true);
        kButtonObject5.SetActive(true);
        kButtonObject6.SetActive(true);
        kButtonObject7.SetActive(true);
    }

}
