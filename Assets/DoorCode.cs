using UnityEngine;

public class Door : MonoBehaviour
{
    public string keyItemName = "Key"; // 키 아이템의 이름
    public GameObject door; // 문 오브젝트
    GameObject itemwindow;

    void Start(){
        itemwindow = GameObject.FindGameObjectWithTag("ItemWindow");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ItemWindow itemWindow = itemwindow.GetComponent<ItemWindow>();
            if (itemWindow != null && itemWindow.HasKeyItem(keyItemName))
            {
                // 키 아이템을 사용하여 문을 엽니다
                itemWindow.UseKeyItem(keyItemName);
                OpenDoor();
            }
        }
    }

    private void OpenDoor()
    {
        // 문을 여는 로직을 여기에 구현합니다. 예: 문 오브젝트를 비활성화
        if (door != null)
        {
            door.SetActive(false);
        }
    }
}
