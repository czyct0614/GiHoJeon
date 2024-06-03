using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject player;
    PlayerMove playercode;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        playercode = player.GetComponent<PlayerMove>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRoomManager.Instance.SetLastTouchedSpawnPoint(transform.position);
            playercode.SavePlayerData(transform.position, playercode.currentHealth);
        }
    }
}
