using UnityEngine;

public class Script : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static Script instance= null;

    void Awake()
    {
        if (null == instance)
        {
            //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            //나는 헷갈림 방지를 위해 this를 붙여주기도 한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
            Destroy(this.gameObject);
        }
    }

    // 스크립트 찾기 함수
    public static T Find<T>(string tag) where T : MonoBehaviour
    {
        // 인스턴스가 없을 경우 새로 생성
        if (instance == null)
        {
            instance = new GameObject("Script").AddComponent<Script>();
        }

        // 해당 태그를 가진 모든 게임 오브젝트를 가져옴
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);

        // 모든 게임 오브젝트를 순회하면서 스크립트를 찾음
        foreach (GameObject obj in taggedObjects)
        {
            T script = obj.GetComponent<T>();
            if (script != null)
            {
                // 스크립트를 찾으면 반환
                return script;
            }
        }

        // 해당 스크립트를 찾지 못한 경우 null 반환
        return null;
    }

    // 스크립트가 삭제될 때 인스턴스도 삭제
    private void OnDestroy()
    {
        instance = null;
    }
}
