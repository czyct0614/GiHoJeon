/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowSkillCooltime : MonoBehaviour
{
    public TextMeshProUGUI Skill1Timer;
    public Image Skill1Disable;

    public TextMeshProUGUI Skill2Timer;
    public Image Skill2Disable;

    public TextMeshProUGUI Skill3Timer;
    public Image Skill3Disable;

    public TextMeshProUGUI UltSkillTimer;
    public Image UltSkillDisable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SkillCoolTimeShow()
    {



        float leftcooldown;

        float cooldown;



        if(spriteRenderer.sprite == newSprite1)
        {

            leftcooldown = Skill1LeftCoolDown;

            cooldown = Skill1CoolDown;

        }
        else if(spriteRenderer.sprite == newSprite2)
        {

            leftcooldown = Skill2LeftCoolDown;

            cooldown = Skill2CoolDown;

        }
        else if(spriteRenderer.sprite == newSprite3)
        {

            leftcooldown = Skill3LeftCoolDown;

            cooldown = Skill3CoolDown;

        }
        else if(spriteRenderer.sprite == newSpriteUltimate)
        {

            leftcooldown = UltimateSkillLeftCoolDown;

            cooldown = UltimateSkillCoolDown;

        }
        else
        {

            leftcooldown=0;

            cooldown=0;

            Debug.Log("스킬 오류");

        }



        if(leftcooldown>0.0f)
        {

            disable.fillAmount = leftcooldown / cooldown;

            int cooltimeSeconds = (int)leftcooldown + 1;

            //string cooltimeText = string.Format("{0:D1}", cooltimeSeconds);

            //timer.text = cooltimeText;.

        }
        else
        {

            disable.fillAmount=0f;

            timer.text = ""; // 쿨타임이 끝나면 텍스트를 비웁니다.

        }



    }
}
*/