using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
     PlayerMove playermove;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");

        playermove = player.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
   {   
    // 충돌한 오브젝트가 몬스터인 경우
    if (other.CompareTag("Player")){
        Debug.Log("플레이어가 데드라인에 닿음(DeadLine 코드 27번 줄)");  
        playermove.dead= true;
        playermove.OnDie();
   }
   else{    
        playermove.dead=false;
   }
    
}

}