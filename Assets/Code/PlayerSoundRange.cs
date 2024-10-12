using UnityEngine;

public class PlayerSoundRange : MonoBehaviour
{
    private int moveInput; // 이동 입력을 저장할 변수
    public int soundAmount; // 소리 양
    public GameObject soundRange;

    private void Awake()
    {

        soundRange = GameObject.FindGameObjectWithTag("SoundRange"); 

    }





    private void Update()
    {

        moveInput = Input.GetKey(KeySetting.keys[KeyAction.Right])?1:Input.GetKey(KeySetting.keys[KeyAction.Left])?-1:0;
        UpdateSoundAmount();

    }





    private void UpdateSoundAmount()
    {

        // 소리 양 업데이트
        soundAmount = 0; // 기본값 초기화

        // A 또는 D 키가 눌렸을 때
        if (moveInput!= 0) // 좌우 이동이 있을 경우
        {
            soundAmount = 6; // 소리 양 설정
        }



        // 추가적인 입력에 따라 소리 양 설정
        if (Input.GetKey(KeySetting.keys[KeyAction.Run]))
        {
            soundAmount = 12;
        }



        if (Input.GetKey(KeySetting.keys[KeyAction.Interact]))
        {
            soundAmount = 6;
        }



        if (Input.GetKey(KeySetting.keys[KeyAction.Kill]))
        {
            soundAmount = 12;
        }



        if (Input.GetKey(KeySetting.keys[KeyAction.Skill]))
        {
            soundAmount = 6;
        }



        if (Input.GetKey(KeySetting.keys[KeyAction.Crouch]))
        {
            soundAmount = 2;
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