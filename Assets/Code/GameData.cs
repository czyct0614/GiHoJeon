// GameData.cs
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string playerMapName;
    public Vector3 playerPosition;
    public int playerHealth;
    public int playerDangerRate;
    public KeyCode[] defaultKeys;

    public GameData(string mapName, Vector2 position, int health, int dangerRate, KeyCode[] Keys)
    {
        this.playerMapName = mapName;
        this.playerPosition = position;
        this.playerHealth = health;
        this.playerDangerRate = dangerRate;
        this.defaultKeys = Keys;
    }
}
