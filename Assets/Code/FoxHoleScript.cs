using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxHoleScript : MonoBehaviour
{

    private GameObject player;
    private NewPlayerCode playermove;
    private PlayerSoundRange playerSoundRange;

    public bool hided = false;
    public bool entered = false;
    private PlayerInput playerInput; // PlayerInput 인스턴스
    
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        playermove = player.GetComponent<NewPlayerCode>();
        playerSoundRange = player.GetComponent<PlayerSoundRange>();

    }





    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Enable(); // 입력 활성화
    }





    private void OnDisable()
    {
        playerInput.Disable(); // 입력 비활성화
    }





    void Update()
    {

        if (hided)
        {
            entered = true;
        }



        if (!hided && playerInput.Player.Interact.triggered && entered)
        {
            playerSoundRange.DeactivateSoundRange();
            playermove.EnableAllBoxColliders(player, false);
            player.gameObject.SetActive(false);
            hided = true;
        }

        else if (hided && playerInput.Player.Interact.triggered && entered)
        {
            player.gameObject.SetActive(true);
            playermove.EnableAllBoxColliders(player, true);
            hided = false;
        }

    }





    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            entered = true;
        }

    }





    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            entered = false;
        }
        
    }

}
