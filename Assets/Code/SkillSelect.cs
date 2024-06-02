using UnityEngine;

public class SkillSelect : MonoBehaviour
{
    ManaSkillImage skillImage; // SkillImage 스크립트에 접근하기 위한 
    //SkillImage skillimage;
    PlayerMove playerMove;
    public Vector2 MouseFinalPos; // 마우스 클릭 종료 위치
    public int SelectedSkillNumber; // 선택된 스킬 번호

    void Start()
    {
        // SkillImage 스크립트를 가진 게임 오브젝트를 찾아서 skillImage에 할당
        GameObject skillimage = GameObject.Find("Skill");
        skillImage = skillimage.GetComponent<ManaSkillImage>();
        //skillImage = skillimage.GetComponent<SkillImage>();

        GameObject playermove = GameObject.Find("Player");
        playerMove = playermove.GetComponent<PlayerMove>();

        // 시간을 느리게
        playerMove.canInput = false;
        Time.timeScale = 0.3f;
    }

    void Update()
    {
        // 마우스 오른쪽 버튼을 뗐을 때
        if (Input.GetMouseButtonUp(1))
        {
            Vector2 skillPos = transform.position;

            // 마우스 클릭 종료 위치를 화면 좌표에서 월드 좌표로 변환하여 정규화(normalized)한 값으로 설정
            MouseFinalPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 마우스 클릭의 각도 계산
            float angle = GetAngle(skillPos, MouseFinalPos);
            angle = angle > 0 ? angle : angle + 360;

            // 선택된 스킬 번호 설정
            if (angle < 45 || angle > 315)
            {
                SelectedSkillNumber = 3;
            }
            else if (angle < 135)
            {
                SelectedSkillNumber = 2;
            }
            else if (angle < 225)
            {
                SelectedSkillNumber = 1;
            }
            else
            {
                SelectedSkillNumber = 4;
            }

            // 선택된 스킬 번호가 0이 아닌 경우에만 스킬 이미지 변경
            if (SelectedSkillNumber != 0)
            {
                skillImage.ChangeSprite(SelectedSkillNumber);
            }

            // 시간을 원래대로 되돌림
            Time.timeScale = 1;
            playerMove.canInput = true;

            // 스킬 선택 오브젝트 파괴
            Destroy(gameObject);
        }
    }

    // 두 점 사이의 각도를 계산하는 함수
    float GetAngle(Vector2 start, Vector2 end)
    {
        Vector2 v2 = end - start;
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }
}
