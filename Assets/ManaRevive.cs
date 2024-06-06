using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaRevive : MonoBehaviour
{
    public bool isFalling = true;
    public float DropPos;
    private Vector2 startPos;
    public bool canCollect=false;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        DropPos=startPos.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            // 아이템을 아래로 이동시킴
            DropPos-=2.6f*Time.deltaTime;
            transform.position = new Vector2(transform.position.x, DropPos);
        }
    }

     // 아이템 창

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            ScriptFinder.FindScriptWithTag<ManaSkillImage>("SelectedSkill").Mana(30);
            Destroy(gameObject);
        }
        if (other.CompareTag("Ground") || other.CompareTag("FlyingPlatform"))
        {
            isFalling = false; // 떨어지는 동작 종료
        }
    }
}
