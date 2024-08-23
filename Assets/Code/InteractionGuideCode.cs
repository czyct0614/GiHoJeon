using UnityEngine;

public class InteractionGuideCode : MonoBehaviour
{
    // 말풍선 프리팹
    public GameObject bubblePrefab; 

    public float offsetX = 0f; 
    public float offsetY = 0f; 
    private GameObject spawnedBubble; // 생성된 말풍선
    private bool hasSpawned = false;




    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 들어온 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (other.CompareTag("Player"))
        {
            // 현재 오브젝트(스프라이트)의 위치
            Vector3 spawnPosition = transform.position + new Vector3(offsetX, offsetY, 0);
            if (!hasSpawned)
            {
                
                // 말풍선 프리팹 생성
                spawnedBubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);
                hasSpawned=true;

            }
        }

    }





    // 플레이어가 트리거에서 나갔을 때 호출되는 메서드
    private void OnTriggerExit2D(Collider2D other)
    {
        // 플레이어가 나가면 생성된 말풍선을 제거
        if (other.CompareTag("Player") && spawnedBubble != null)
        {
            Destroy(spawnedBubble);
            hasSpawned=false;
        }

    }

}