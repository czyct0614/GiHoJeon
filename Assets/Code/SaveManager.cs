using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private DangerRate dangerRate;
    private NewPlayerCode newPlayerCode;
    private KeyManager keyManager;
    private GameObject player;

    private void Start()
    {
        // 씬에 있는 dangerRate와 newPlayerCode 스크립트를 참조
        dangerRate = FindObjectOfType<DangerRate>();
        newPlayerCode = FindObjectOfType<NewPlayerCode>();
        keyManager = FindObjectOfType<KeyManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // 게임 데이터를 저장하는 함수
    public void SaveGame()
    {
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

            // 불러온 데이터를 각 스크립트에 적용
            newPlayerCode.nowMap = data.playerMapName;
            player.transform.position = data.playerPosition;
            newPlayerCode.currentHealth = data.playerHealth;
            dangerRate.dangerRate = data.playerDangerRate;
            keyManager.defaultKeys = data.defaultKeys;

            return data;
        }
        return null;
    }
}
