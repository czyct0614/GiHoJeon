using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class SkillCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    void CastHackSkill()
    {

        // 마우스 포인터 위치에서 레이를 발사합니다.
        Vector2 rayPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D[] hits = Physics2D.RaycastAll(rayPosition, Vector2.zero);

        if (hits.Length > 0)
        {
            // 레이캐스트로 맞은 오브젝트들 중에서 맨 위에 있는 몬스터를 찾습니다.
            foreach (RaycastHit2D hit in hits)
            {
                
                // 충돌한 컬라이더가 있는지 확인합니다.
                if (hit.collider != null) 
                {
                    // 태그에 따라 적절한 컴포넌트를 가져옵니다.
                    var enemycode = hit.transform.GetComponent(hit.transform.tag + "Move");

                    if (enemycode != null)
                    {
                        FieldInfo hackedField = enemycode.GetType().GetField("hacked");

                        // 'hacked' 필드의 값을 true로 설정합니다.
                        hackedField.SetValue(enemycode, true);

                        // 맨 위에 있는 몬스터만 해킹하기 때문에 반복문을 종료합니다.
                        break;
                    }

                }

            }

        }

    }

}