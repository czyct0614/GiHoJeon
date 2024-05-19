using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settingcode : MonoBehaviour
{

    public GameObject settingsPanel;
    public GameObject escPanel;

    // 카메라 중심
    private Vector3 cameraCenter;
    // Renderer 컴포넌트
    private Renderer myRenderer;
    public GameObject SbuttonObject;
    public GameObject SbuttonObject2;
    public GameObject SbuttonObject3;
    public GameObject VolumePanel;
    public bool VisActive;

    // Start is called before the first frame update
    void Start()
    {
        // 버튼 비활성화
        SbuttonObject.SetActive(false);
        SbuttonObject2.SetActive(false);
        SbuttonObject3.SetActive(false);
        settingsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        VisActive = VolumePanel.activeSelf;

        if (Input.GetButtonDown("Escape") && !VisActive){
            settingsPanel.SetActive(false);
            escPanel.SetActive(true);
        }
        else{
            escPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }
    }

    public void SettingPressed(){
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
        SbuttonObject.SetActive(true);
        SbuttonObject2.SetActive(true);
        SbuttonObject3.SetActive(true);
    }

    public void respawnSetting()
    {
        myRenderer.enabled = true;

        // 버튼 상태
        SbuttonObject.SetActive(false);
        SbuttonObject2.SetActive(false);
        SbuttonObject3.SetActive(false);
    }
}
