using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Door : MonoBehaviour
{
    public string keyItemName = "Key"; // 키 아이템의 이름
    public GameObject door; // 문 오브젝트
    GameObject itemwindow;

    
    public GameObject DoorAxis;
    public float rotationSpeed;
    private const float maxRotationValue = 90f;
    private const float FLOAT_COMPARISON_VALUE = 2f;
    private WaitForSeconds gatePassWaitTime = new WaitForSeconds(0.5f);
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
            StartCoroutine(GateOpen());
        }
    }

    IEnumerator GateOpen()
    {

        while (true)
        {
            float DoorCurEulerZ = DoorAxis.transform.eulerAngles.z;
            bool isCompleteOpen =_Approximately(DoorCurEulerZ, maxRotationValue);

            if (isCompleteOpen)
            {
                break;
            }

            DoorAxis.transform.eulerAngles =
            new Vector3(0f,0f, Mathf.LerpAngle( DoorCurEulerZ,
                                                maxRotationValue, 
                                                Time.deltaTime * rotationSpeed));
            yield return null;
        }
    }

    private bool _Approximately(float x, float y)
    {
        float absCalcValue = (x - y) > 0f ? x - y : y - x;
        if (absCalcValue <= FLOAT_COMPARISON_VALUE)
            return true;
        return false;
    }
}