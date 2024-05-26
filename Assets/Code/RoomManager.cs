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
            DontDestroyOnLoad(gameObject);  // 씬이 로드될 때 오브젝트가 파괴되지 않도록 합니다.
        }
        else
        {
            Destroy(gameObject);  // 이미 인스턴스가 존재하면 중복 생성을 방지합니다.
        }
    }

    public void SetLastTouchedSpawnPoint(Vector2 spawnPoint)
    {
        lastTouchedSpawnPoint = spawnPoint;
        Debug.Log("마지막으로 닿은 스폰포인트가 설정되었습니다: " + spawnPoint);
    }

    public Vector2 GetLastTouchedSpawnPoint()
    {
        return lastTouchedSpawnPoint;
    }
}
