using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public DangerRate dangerRate;
    public NewPlayerCode newPlayerCode;
    public KeyManager keyManager;
    public GameObject player;

    private void Start()
    {

        if (dangerRate == null)
        {
            Debug.LogError("DangerRate script not found.");
        }
        if (newPlayerCode == null)
        {
            Debug.LogError("NewPlayerCode script not found.");
        }
        if (keyManager == null)
        {
            Debug.LogError("KeyManager script not found.");
        }

        if (player == null)
        {
            Debug.LogError("Player GameObject not assigned.");
        }
    }

    public void StartGame()
    {
        GameData data = new GameData(
            "Chapter 1",
            new Vector3(-22, 0, 0),
            10,
            0,
            new KeyCode[] { KeyCode.A, KeyCode.D, KeyCode.Q, KeyCode.LeftShift, KeyCode.LeftControl, KeyCode.E, KeyCode.F }
        );

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "gamedata.json"), json);
        LoadGame();
    }

    // 게임 데이터를 저장하는 함수
    public void SaveGame()
    {
        // player와 스크립트들이 null이 아닌지 확인
        if (player == null || newPlayerCode == null || dangerRate == null || keyManager == null)
        {
            Debug.LogError("Cannot save game: one or more components are not assigned.");
            return;
        }

        // 플레이어와 레벨 데이터를 가져와서 저장
        GameData data = new GameData(
            newPlayerCode.nowMap,
            player.transform.position,
            newPlayerCode.currentHealth,
            dangerRate.dangerRate,
            keyManager.defaultKeys
        );

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "gamedata.json"), json);
    }

    // 게임 데이터를 불러오는 함수
    public GameData LoadGame()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameData data = JsonUtility.FromJson<GameData>(json);

            // 불러온 데이터를 각 스크립트에 적용하기 전에 null 체크
            if (newPlayerCode != null)
            {
                newPlayerCode.nowMap = data.playerMapName;
                newPlayerCode.startpoint = data.playerPosition;
                newPlayerCode.currentHealth = data.playerHealth;
            }
            else
            {
                Debug.LogError("NewPlayerCode script is not assigned.");
            }

            if (dangerRate != null)
            {
                dangerRate.dangerRate = data.playerDangerRate;
            }
            else
            {
                Debug.LogError("DangerRate script is not assigned.");
            }

            if (keyManager != null)
            {
                keyManager.defaultKeys = data.defaultKeys;
            }
            else
            {
                Debug.LogError("KeyManager script is not assigned.");
            }

            return data;
        }
        else
        {
            Debug.LogError("Save file not found at " + filePath);
        }

        return null;
    }
}
