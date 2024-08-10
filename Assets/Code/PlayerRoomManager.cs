using UnityEngine;

public class PlayerRoomManager : MonoBehaviour
{

    public static PlayerRoomManager Instance;

    private Vector2 lastTouchedSpawnPoint;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            // 씬이 로드될 때 오브젝트가 파괴되지 않도록 합니다.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재하면 중복 생성을 방지합니다.
            Destroy(gameObject);

        }
    }





    public void SetLastTouchedSpawnPoint(Vector2 spawnPoint)
    {

        lastTouchedSpawnPoint = spawnPoint;

    }





    public Vector2 GetLastTouchedSpawnPoint()
    {

        return lastTouchedSpawnPoint;

    }

}
