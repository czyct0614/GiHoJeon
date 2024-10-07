using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settingcode : MonoBehaviour
{

    public GameObject settingsPanel;
    public GameObject escPanel;
    public GameObject sButtonObject1;
    public GameObject sButtonObject2;
    public GameObject sButtonObject3;
    public GameObject kButtonObject1;
    public GameObject kButtonObject2;
    public GameObject kButtonObject3;
    public GameObject kButtonObject4;
    public GameObject kButtonObject5;
    public GameObject kButtonObject6;
    public GameObject kButtonObject7;
    public GameObject volumePanel;
    public GameObject KeyPanel;

    // 카메라 중심
    private Vector3 cameraCenter;
    // Renderer 컴포넌트
    private Renderer myRenderer;

    public bool VisActive;
    public bool KisActive;

    void Start()
    {

        // 버튼 비활성화
        sButtonObject1.SetActive(false);
        sButtonObject2.SetActive(false);
        sButtonObject3.SetActive(false);
        kButtonObject1.SetActive(false);
        kButtonObject2.SetActive(false);
        kButtonObject3.SetActive(false);
        kButtonObject4.SetActive(false);
        kButtonObject5.SetActive(false);
        kButtonObject6.SetActive(false);
        kButtonObject7.SetActive(false);
        settingsPanel.SetActive(false);
        KeyPanel.SetActive(false);

    }





    void Update()
    {

        VisActive = volumePanel.activeSelf;
        KisActive = KeyPanel.activeSelf;

        if (Input.GetButtonDown("Escape") && !VisActive)
        {
            settingsPanel.SetActive(false);
            escPanel.SetActive(true);
        }
        else
        {
            escPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

    }





    public void SettingPressed()
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

        // 버튼 상태
        sButtonObject1.SetActive(true);
        sButtonObject2.SetActive(true);
        sButtonObject3.SetActive(true);

    }





    public void respawnSetting()
    {
        myRenderer = GetComponent<Renderer>();
        myRenderer.enabled = true;

        // 버튼 상태
        sButtonObject1.SetActive(false);
        sButtonObject2.SetActive(false);
        sButtonObject3.SetActive(false);

    }





    public void respawnKey()
    {
        myRenderer = GetComponent<Renderer>();
        myRenderer.enabled = true;

        // 버튼 상태
        //sButtonObject1.SetActive(false);
        //sButtonObject2.SetActive(false);
        //sButtonObject3.SetActive(false);

    }

}
