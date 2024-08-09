using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    Vector3 cameraPosition;

    [SerializeField]
    float cameraMoveSpeed;
    float height;
    float width;

    public RoomBounds? currentRoomBounds = null; // 현재 방의 경계를 저장하는 변수

    public static CameraController Instance;

    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();

        height = Camera.main.orthographicSize;
        width = height * 16/9;
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject); // 다른 맵에서 없어지지 않게 해줌
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "StartScene") {
            transform.position = new Vector3(0, -3, -10);
        }
        LimitCameraArea();
    }

    void LimitCameraArea()
    {
        RoomBounds bounds = currentRoomBounds.Value;

        transform.position = Vector3.Lerp(transform.position, 
                                          playerTransform.position + cameraPosition, 
                                          Time.deltaTime * cameraMoveSpeed);
        float lx = bounds.MapSizeX/2 - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + bounds.CenterX, lx + bounds.CenterX);

        float ly = bounds.MapSizeY/2 - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + bounds.CenterY, ly + bounds.CenterY);

        transform.position = new Vector3(clampX, clampY, -10f);
    }

    public void SetCurrentRoom(RoomBounds bounds) {
        currentRoomBounds = bounds;
    }

    public void ClearCurrentRoom() {
        currentRoomBounds = null;
    }

    public string GetCurrentRoomName() {
        return currentRoomBounds?.roomName;
    }
}