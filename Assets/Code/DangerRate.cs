using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DangerRate : MonoBehaviour
{



    [SerializeField] private Image dangerBarImage;



    // Start is called before the first frame update
    void Start()
    {
        SetDangerRate(0);
    }



    public void SetDangerRate(int num)
    {
        if(num!=0&&num!=1&&num!=2&&num!=3&&num!=4&&num!=5)
        {
            Debug.LogWarning("경고 값은 0에서 5까지 정수로만 하기");
            return;
        }
        if(dangerBarImage != null)
        {

            dangerBarImage.fillAmount = (float)num/5;

        }
        switch (num)
        {
            case 1:
                dangerBarImage.color = Color.blue; // 파랑
                break;
            case 2:
                dangerBarImage.color = Color.green; // 초록
                break;
            case 3:
                dangerBarImage.color = Color.yellow; // 노랑
                break;
            case 4:
                dangerBarImage.color = new Color(1f, 0.647f, 0f); // 주황
                break;
            case 5:
                dangerBarImage.color = Color.red; // 빨강
                break;
            default:
                dangerBarImage.color = Color.white; // 기본값 (색상 코드가 유효하지 않은 경우)
                break;
        }
    }



    public void ChangeDangerRate(int num)
    {
        int originalRate=(int)(dangerBarImage.fillAmount*5);
        int finalRate=originalRate+num;
        if(finalRate>=5)
        {
            finalRate=5;
        }
        else if(finalRate<=0)
        {
            finalRate=0;
        }


        if(dangerBarImage != null)
        {
            dangerBarImage.fillAmount = (float)finalRate/5;
        }



        switch (finalRate)
        {
            case 1:
                dangerBarImage.color = Color.blue; // 파랑
                break;
            case 2:
                dangerBarImage.color = Color.green; // 초록
                break;
            case 3:
                dangerBarImage.color = Color.yellow; // 노랑
                break;
            case 4:
                dangerBarImage.color = new Color(1f, 0.647f, 0f); // 주황
                break;
            case 5:
                dangerBarImage.color = Color.red; // 빨강
                break;
            default:
                dangerBarImage.color = Color.white; // 기본값 (색상 코드가 유효하지 않은 경우)
                break;
        }
    }



    public int CheckDangerRate()
    {
        return (int)(dangerBarImage.fillAmount*5);
    }



}
