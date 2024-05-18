using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public float floatHeight = 0.5f; // 아이템이 움직일 높이
    public float floatSpeed = 3f; 
    public bool Eaten=false;
    private Vector2 startPos;
    private Quaternion originalRotation;
    public GameObject itemWindow;
    public bool isFalling = true;
    public float DropPos;
    public bool canCollect=false;
    public int IndexNum;
    private SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        DropPos=startPos.y;
        originalRotation = transform.rotation;
        itemWindow = GameObject.FindGameObjectWithTag("ItemWindow");
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        else if(!Eaten){
            // 아이템이 위아래로 움직이도록 설정
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = new Vector2(transform.position.x, newY);

            // 아이템을 회전시킵니다.
            transform.Rotate(new Vector3(0,1,0), Time.deltaTime * 100f);
        }
        if (Input.GetKeyDown(KeyCode.F)&&canCollect)
            {
                Debug.Log("F key pressed.");
                PickUpItem();
            }
    }

     // 아이템 창

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canCollect=true;
            //Debug.Log("Press 'F' to pick up the item.");
        }
        if (other.CompareTag("Ground") || other.CompareTag("FlyingPlatform"))
        {
            isFalling = false; // 떨어지는 동작 종료
            startPos.y = transform.position.y + floatHeight+0.1f;
        }
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canCollect=false;
        }
    }

    private void PickUpItem()
    {
        // 아이템을 아이템 창에 추가
        if (itemWindow != null)
        {
            ItemWindow itemWindowScript = itemWindow.GetComponent<ItemWindow>();
            if (itemWindowScript != null)
            {
                Eaten=true;
                isFalling=false;
                canCollect=false;
                transform.rotation = originalRotation;
                DontDestroyOnLoad(this.gameObject);
                spriteRenderer.sortingOrder = 99;
                itemWindowScript.EquipItem(gameObject,IndexNum);
                // 아이템 창에 들어갔으므로 아이템 비활성화
                Destroy(gameObject); // 현재 스프라이트를 포함하는 GameObject를 파괴합니다.
                gameObject.SetActive(false);

            }
        }
    }
}