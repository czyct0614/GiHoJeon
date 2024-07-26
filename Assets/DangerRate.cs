using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DangerRate : MonoBehaviour
{
    [SerializeField] private Image DangerBarImage;
    // Start is called before the first frame update
    void Start()
    {
        SetDangerRate(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDangerRate(int num)
    {
        if(num!=1&&num!=2&&num!=3&&num!=4&&num!=5){
            Debug.LogWarning("경고 값은 1에서 5까지 정수로만 하기");
            return;
        }
        if(DangerBarImage != null){

            DangerBarImage.fillAmount = (float)num/5;

        }
        switch (num)
        {
            case 1:
                DangerBarImage.color = Color.red; // 빨강
                break;
            case 2:
                DangerBarImage.color = new Color(1f, 0.647f, 0f); // 주황
                break;
            case 3:
                DangerBarImage.color = Color.yellow; // 노랑
                break;
            case 4:
                DangerBarImage.color = Color.green; // 초록
                break;
            case 5:
                DangerBarImage.color = Color.blue; // 파랑
                break;
            default:
                DangerBarImage.color = Color.white; // 기본값 (색상 코드가 유효하지 않은 경우)
                break;
        }
    }



    public void ChangeDangerRate(int num)
    {
        int OriginalRate=(int)(DangerBarImage.fillAmount*5);
        int FinalRate=OriginalRate+num;
        if(FinalRate>=5)
        {
            FinalRate=5;
        }
        else if(FinalRate<=0)
        {
            FinalRate=0;
        }


        if(DangerBarImage != null)
        {
            DangerBarImage.fillAmount = (float)FinalRate/5;
        }



        switch (FinalRate)
        {
            case 1:
                DangerBarImage.color = Color.red; // 빨강
                break;
            case 2:
                DangerBarImage.color = new Color(1f, 0.647f, 0f); // 주황
                break;
            case 3:
                DangerBarImage.color = Color.yellow; // 노랑
                break;
            case 4:
                DangerBarImage.color = Color.green; // 초록
                break;
            case 5:
                DangerBarImage.color = Color.blue; // 파랑
                break;
            default:
                DangerBarImage.color = Color.white; // 기본값 (색상 코드가 유효하지 않은 경우)
                break;
        }
    }
    public int CheckDangerRate()
    {
        return (int)(DangerBarImage.fillAmount*5);
    }
}
