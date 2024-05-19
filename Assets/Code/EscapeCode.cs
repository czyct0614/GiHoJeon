using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeCode : MonoBehaviour
{
    // 카메라 중심
    private Vector3 cameraCenter;
    // Renderer 컴포넌트
    private Renderer myRenderer;
    private bool esc = false;
    public GameObject buttonObject;
    public GameObject buttonObject2;
    public GameObject buttonObject3;
    public GameObject Skillselect;
    private AudioSource musicSource; // 음악을 재생하는 AudioSource
    private float pausedTime; // 멈춘 시간

    void Start()
    {
        GameObject MusicSource = GameObject.FindGameObjectWithTag("Music");
        musicSource = MusicSource.GetComponent<AudioSource>();
        // Renderer 컴포넌트 가져오기
        myRenderer = GetComponent<Renderer>();
        // 스프라이트 숨기기
        myRenderer.enabled = false;
        // 버튼 비활성화
        buttonObject.SetActive(false);
        buttonObject2.SetActive(false);
        buttonObject3.SetActive(false);
        // 처음에 음악을 재생합니다.
        PlayMusic();
    }

    void Update()
    {
        if(!musicSource){
        GameObject MusicSource = GameObject.FindGameObjectWithTag("Music");
        musicSource = MusicSource.GetComponent<AudioSource>();
        PlayMusic();
        }
        // 카메라 중심 계산
        Vector3 cameraCenter = Camera.main.transform.position;
        // 스프라이트의 상대 위치 계산
        Vector3 offset = transform.position - cameraCenter;
        // 이동할 위치 계산 (카메라와 동일한 x, 현재 y 좌표, z 좌표)
        Vector3 targetPosition = new Vector3(cameraCenter.x, cameraCenter.y, transform.position.z);

        if (Input.GetButtonDown("Escape"))
        {
            // SkillSelect GameObject 찾기
            Skillselect = GameObject.FindWithTag("SkillSelect");
            // 스프라이트 이동 (카메라 중심으로)
            transform.position = targetPosition;
            // 스프라이트 토글
            myRenderer.enabled = !myRenderer.enabled;
            // esc 토글
            esc = !esc;
            // 버튼 상태 토글
            buttonObject.SetActive(!buttonObject.activeSelf);
            buttonObject2.SetActive(!buttonObject2.activeSelf);
            buttonObject3.SetActive(!buttonObject3.activeSelf);
            // 로그 출력
            //Debug.Log(!esc);
            //Debug.Log(Skillselect);
            // 일시 정지
            if (esc)
            {
                Time.timeScale = 0f;
                PauseMusic();
            }
            // 일시 정지 해제
            else if (!esc && Skillselect == null)
            {
                Time.timeScale = 1f;
                PlayMusic();
            }
        }
    }

    // 다시 시작할 때 실행되는 함수
    public void respawnEscape()
    {
        // 스프라이트 숨기기
        myRenderer.enabled = false;
        // esc 초기화
        esc = false;
        // 시간 비율 초기화
        Time.timeScale = 1f;
        // 버튼 상태 토글
        buttonObject.SetActive(!buttonObject.activeSelf);
        buttonObject2.SetActive(!buttonObject2.activeSelf);
        buttonObject3.SetActive(!buttonObject3.activeSelf);
        PlayMusic();
    }

    public void PlayMusic()
    {
        // 멈춘 시간이 0이 아니라면, 멈춘 시간부터 재생합니다.
        if (pausedTime != 0)
        {
            musicSource.time = pausedTime;
        }
        // 음악을 재생합니다.
        musicSource.Play();
    }

    public void PauseMusic()
    {
        // 음악을 멈추고, 현재 재생 위치를 기억합니다.
        musicSource.Pause();
        pausedTime = musicSource.time;
    }
    public void StopMusic()
    {
        // 음악을 멈추고, 멈춘 시간을 초기화합니다.
        musicSource.Stop();
        pausedTime = 0f;
    }
}
