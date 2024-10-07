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


    
    



    void Start()
    {

        updoormove = updoorObject.GetComponent<UpDoorMove>();

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

            Debug.Log(other.tag);
            if (other.CompareTag(targetTag) )
            {

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
                
                doorManagerCode.detected = false;
                
                break; 
                
            }
             
        }

    }

}
