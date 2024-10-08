using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSoundRange : MonoBehaviour
{
    private PlayerInput playerInput; // PlayerInput 인스턴스
    private Vector2 moveInput; // 이동 입력을 저장할 변수
    private int soundAmount; // 소리 양
    public GameObject soundRange;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();// 입력 활성화
        soundRange = GameObject.FindGameObjectWithTag("SoundRange"); 
    }





    private void OnEnable()
    {
        playerInput.Enable(); // 입력 활성화
    }





    private void OnDisable()
    {
        playerInput.Disable(); // 입력 비활성화
    }





    private void Update()
    {
        moveInput = playerInput.Player.Move.ReadValue<Vector2>(); // 이동 입력 읽기
        UpdateSoundAmount();
    }





    private void UpdateSoundAmount()
    {
        // 소리 양 업데이트
        soundAmount = 0; // 기본값 초기화

        // A 또는 D 키가 눌렸을 때
        if (moveInput.x != 0) // 좌우 이동이 있을 경우
        {
            soundAmount = 5; // 소리 양 설정
        }

        // 추가적인 입력에 따라 소리 양 설정
        if (playerInput.Player.Run.IsPressed())
        {
            soundAmount = 10;
        }

        if (playerInput.Player.Interact.IsPressed())
        {
            soundAmount = 7;
        }

        if (playerInput.Player.Kill.IsPressed())
        {
            soundAmount = 5;
        }

        if (playerInput.Player.Skill.IsPressed())
        {
            soundAmount = 5;
        }

        if(soundAmount == 0)
        {
            DeactivateSoundRange();
        }
        else{
            ActivateSoundRange();
        }
    }
    




    private void ActivateSoundRange()
    {

        soundRange.transform.localScale = new Vector3 (soundAmount, 3, 5);
        soundRange.SetActive(true);

    }





// 소리범위 끄는 함수
    public void DeactivateSoundRange()
    {

        soundRange.SetActive(false);

    }
}
