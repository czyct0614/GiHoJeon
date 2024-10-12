using UnityEngine;

public class DoorDetectRangeCode : MonoBehaviour
{
    // 감지할 오브젝트의 태그 배열
    private string[] targetTags;

    private DoorManagerCode doorManagerCode;

    private UpDoorMove updoormove;
    private DownDoorMove downdoormove;

    public GameObject updoorObject;
    public GameObject downdoorObject;
    
    public GameObject newEnemyObject;
    public NewEnemyCode newEnemyCode;


    
    



    void Start()
    {

        updoormove = updoorObject.GetComponent<UpDoorMove>();

        newEnemyObject = GameObject.FindGameObjectWithTag("NewEnemy");

        newEnemyCode = newEnemyObject.GetComponent<NewEnemyCode>();

        downdoormove = downdoorObject.GetComponent<DownDoorMove>();

        doorManagerCode = GetComponentInParent<DoorManagerCode>();

        if (doorManagerCode == null)
        {
            Debug.LogError("부모 오브젝트에 DoorManagerCode 스크립트가 없습니다.");
        }

        doorManagerCode.detected = false;
        
        

        targetTags = new string[] { "Player", "NewEnemy" };

    }





    void OnTriggerEnter2D(Collider2D other)
    {
       
        // 들어온 오브젝트의 태그가 배열에 있는지 확인
        foreach (string targetTag in targetTags )
        {
        // 콜라이더 범위 안에 들어왔을때 적의 상태가 순찰중이라면 문이 열리지 않는다
            Debug.Log(other.tag);
            if (other.CompareTag(targetTag))
            {   
                
                updoormove.StopCoroutine("UpDone");
                downdoormove.StopCoroutine("DownDone");
                
                if (targetTag == "Player")

                   doorManagerCode.detected = true;


                if (targetTag == "NewEnemy" && !newEnemyCode.patrolling) 
                 
                   doorManagerCode.detected = true;


              break;

            }

        }

    }





    void OnTriggerExit2D(Collider2D other)
    {

        foreach (string targetTag in targetTags)
        {

            if (other.CompareTag(targetTag))
            {
                
                updoormove.StopCoroutine("Up");
                downdoormove.StopCoroutine("Down");
                doorManagerCode.detected = false;
                
                break; 
                
            }
             
        }

    }

}
