/*using UnityEngine;

public class ItemWindow : MonoBehaviour
{
    public GameObject[] itemSlots; // 아이템 슬롯 배열
    public GameObject[] equippedItems; // 장착된 아이템 배열

    public Transform cameraTransform; // 플레이어의 위치

    void Start()
    {
        DontDestroyOnLoad(this.gameObject); // 맵 전환 시에도 유지되도록 설정
        /*itemSlots = new GameObject[2];
        // 장착된 아이템 배열 초기화
        equippedItems = new GameObject[itemSlots.Length];
    }

    void Update()
    {
        transform.position = new Vector3(cameraTransform.position.x +13.52f, cameraTransform.position.y-8.43f,cameraTransform.position.z+10f);
        // 아이템 슬롯이 플레이어를 따라다니도록 설정
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i]){
            itemSlots[i].transform.position = GetItemSlotPosition(i);
            }
        }
    }

    // 아이템 슬롯의 위치를 플레이어를 기준으로 계산하는 함수
    Vector3 GetItemSlotPosition(int index)
    {
        if (cameraTransform == null)
        {
            Debug.LogWarning("Player transform is null.");
            return Vector3.zero;
        }

        float xOffset = 0f;
        // 첫 번째 아이템 슬롯의 경우
        if (index == 0)
        {
            xOffset = 12f;
        }
        // 두 번째 아이템 슬롯의 경우
        else if (index == 1)
        {
            xOffset = 14.63f;
        }
        return new Vector3(cameraTransform.position.x + xOffset, cameraTransform.position.y - 9f, cameraTransform.position.z + 10f);
    }

    // 아이템을 장착하는 메서드
    public void EquipItem(GameObject itemPrefab, int slotIndex)
    {
        // 선택한 슬롯에 아이템이 장착되어 있지 않은 경우에만 아이템을 장착
        if (slotIndex >= 0 && slotIndex < equippedItems.Length)
        {
            // 선택한 슬롯에 이미 아이템이 장착되어 있는 경우
            if (equippedItems[slotIndex] != null)
            {
                // 기존 아이템을 제거하고 새로운 아이템을 장착
                Destroy(equippedItems[slotIndex]); // 기존 아이템 제거
                equippedItems[slotIndex] = Instantiate(itemPrefab, GetItemSlotPosition(slotIndex), Quaternion.identity, transform);
                equippedItems[slotIndex].transform.SetParent(itemSlots[slotIndex].transform); // 아이템의 부모를 아이템 슬롯으로 설정
            }
            else
            {
                // 선택한 슬롯에 아이템을 장착
                equippedItems[slotIndex] = Instantiate(itemPrefab, GetItemSlotPosition(slotIndex), Quaternion.identity, transform);
                equippedItems[slotIndex].transform.SetParent(itemSlots[slotIndex].transform); // 아이템의 부모를 아이템 슬롯으로 설정
            }
        }
        else
        {
            Debug.LogWarning("Invalid slot index.");
        }
    }
}
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemWindow : MonoBehaviour
{
    public GameObject[] itemSlots; // 아이템 슬롯 배열
    public GameObject[] equippedItems; // 장착된 아이템 배열

    public Transform cameraTransform; // 플레이어의 위치

    void Start()
    {
        equippedItems = new GameObject[itemSlots.Length];
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    void Update()
    {
        //transform.position = new Vector3(cameraTransform.position.x + 32.4f, cameraTransform.position.y - 14.8f, cameraTransform.position.z + 10f);

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i])
            {
                itemSlots[i].transform.position = GetItemSlotPosition(i);
            }
        }

        foreach (GameObject items in itemSlots)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName == "StartScene")
            {
                Destroy(items);
            }
        }
    }

    Vector3 GetItemSlotPosition(int index)
    {
        if (cameraTransform == null)
        {
            Debug.LogWarning("Player transform is null.");
            return Vector3.zero;
        }

        float xOffset = 0f;
        if (index == 0)
        {
            xOffset = 12f;
        }
        else if (index == 1)
        {
            xOffset = 14.63f;
        }
        return new Vector3(cameraTransform.position.x + xOffset, cameraTransform.position.y - 9f, cameraTransform.position.z + 10f);
    }

    public void EquipItem(GameObject itemPrefab, int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < equippedItems.Length)
        {
            if (equippedItems[slotIndex] != null)
            {
                DroppingItem(equippedItems[slotIndex]);
                equippedItems[slotIndex] = Instantiate(itemPrefab, GetItemSlotPosition(slotIndex), Quaternion.identity, transform);
                equippedItems[slotIndex].transform.SetParent(itemSlots[slotIndex].transform);
            }
            else
            {
                equippedItems[slotIndex] = Instantiate(itemPrefab, GetItemSlotPosition(slotIndex), Quaternion.identity, transform);
                equippedItems[slotIndex].transform.SetParent(itemSlots[slotIndex].transform);
            }
        }
        else
        {
            Debug.LogWarning("Invalid slot index.");
        }
    }

    private void DroppingItem(GameObject item)
    {
        GameObject droppedItem = Instantiate(item, cameraTransform.position, Quaternion.identity);
        DropItem dropItemComponent = droppedItem.GetComponent<DropItem>();
        dropItemComponent.isFalling = true;
        dropItemComponent.eaten = false;

        for (int i = 0; i < equippedItems.Length; i++)
        {
            if (equippedItems[i] == item)
            {
                equippedItems[i] = null;
                break;
            }
        }

        Destroy(item);
    }

    public bool HasKeyItem(string keyItemName)
    {
        foreach (GameObject item in equippedItems)
        {
            if (item != null && item.name.Contains(keyItemName))
            {
                return true;
            }
        }
        return false;
    }

    public void UseKeyItem(string keyItemName)
    {
        for (int i = 0; i < equippedItems.Length; i++)
        {
            if (equippedItems[i] != null && equippedItems[i].name.Contains(keyItemName))
            {
                Destroy(equippedItems[i]);
                equippedItems[i] = null;
                break;
            }
        }
    }
}

