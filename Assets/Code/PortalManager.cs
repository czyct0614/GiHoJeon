using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public int totalMonsters; // 현재 스테이지에 있는 총 몬스터 수
    private int deadMonsters = 0; // 죽은 몬스터 수
    public GameObject portalPrefab;
    private bool IsPortal = false;
    public Vector3 chuemwhich=new Vector3(40f,-8f,0f);

    private void Update()
    {
        // 모든 몬스터가 죽었는지를 확인하는 함수를 호출하여 반환된 결과가 true인 경우에만 포탈을 활성화함
        if (AllEnemiesDead())
        {
            // 포탈을 활성화하여 다음 스테이지로 이동할 수 있도록 함
            GameObject portal = Instantiate(portalPrefab,chuemwhich, transform.rotation);
            IsPortal=true;
        }
    }

    // 몬스터가 죽었을 때 호출되는 함수
    public void MonsterDied()
    {
        // 죽은 몬스터 수 증가
        deadMonsters++;
    }

    // 모든 몬스터가 죽었는지 여부를 반환하는 함수
    bool AllEnemiesDead()
    {
        if(IsPortal){
            return false;
        }
        // 죽은 몬스터 수와 총 몬스터 수를 비교하여 모든 몬스터가 죽었는지 판별
        return deadMonsters >= totalMonsters;
    }
}