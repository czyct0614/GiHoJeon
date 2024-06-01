using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour {
    [SerializeField] float smoothing = 0.2f;
    float maxVerticalOffset = 5f; // 카메라가 위로 이동하는 기준
    float minY = -3f;

    public static CameraFollow Instance;

    private Transform player; // 플레이어의 Transform
    public Vector3 OriginalPosition;

    public RoomBounds? currentRoomBounds = null; // 현재 방의 경계를 저장하는 변수
    private bool isFollowing = true;

    private void Awake() {
        DontDestroyOnLoad(gameObject); // 다른 맵에서 없어지지 않게 해줌
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        FindPlayer();
        transform.position = new Vector3(transform.position.x, minY, transform.position.z);
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        FindPlayer();
    }

    private void FindPlayer() {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) {
            player = playerObject.transform;
        }
    }

    private void FixedUpdate() {
        FindPlayer();

        if (SceneManager.GetActiveScene().name == "StartScene") {
            //Debug.Log("a");
            transform.position = new Vector3(0, -3, -10);
        } else if (player != null && currentRoomBounds.HasValue) {
            //Debug.Log("b");
            // 캐릭터와 카메라의 거리 계산
            float verticalOffset = player.position.y - transform.position.y;
            float horizontalOffset = player.position.x - transform.position.x;

            // 캐릭터가 일정 오프셋 이상 위로 이동했을 때만 카메라를 따라감
            if (Mathf.Abs(verticalOffset) > maxVerticalOffset) {
                isFollowing = true;
            }

            UpdateCameraPosition(horizontalOffset);
            OriginalPosition = transform.position;
        } else {
            Debug.Log("currentRoomBounds.HasValue: " + currentRoomBounds.HasValue);
            Debug.Log("Current room name: " + GetCurrentRoomName());
        }
    }

    private void UpdateCameraPosition(float horizontalOffset) {
        RoomBounds bounds = currentRoomBounds.Value;

        if (isFollowing) {
            // 카메라 이동
            Vector3 targetPosition = new Vector3(transform.position.x, player.position.y - maxVerticalOffset, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);

            // 카메라가 방의 경계를 넘어가지 않도록 함
            if (transform.position.y > bounds.maxY) {
                transform.position = new Vector3(transform.position.x, bounds.maxY, transform.position.z);
                isFollowing = false;
            }
            if (transform.position.y < bounds.minY) {
                transform.position = new Vector3(transform.position.x, bounds.minY, transform.position.z);
                isFollowing = false;
            }
            if (transform.position.x > bounds.maxX) {
                transform.position = new Vector3(bounds.maxX, transform.position.y, transform.position.z);
                isFollowing = false;
            }
            if (transform.position.x < bounds.minX) {
                transform.position = new Vector3(bounds.minX, transform.position.y, transform.position.z);
                isFollowing = false;
            }
        }

        // x축 이동 처리
        Vector3 horizontalTargetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, horizontalTargetPosition, smoothing);
    }

    public void SetCurrentRoom(RoomBounds bounds) {
        Debug.Log("SetCurrentRoom called with bounds: " + bounds.roomName);
        currentRoomBounds = bounds;
        Debug.Log("currentRoomBounds.HasValue: " + currentRoomBounds.HasValue);
        isFollowing = true;
        Debug.Log("Current room name after setting: " + GetCurrentRoomName());
    }

    public void ClearCurrentRoom() {
        Debug.Log("ClearCurrentRoom called");
        currentRoomBounds = null;
        isFollowing = true;
    }

    public string GetCurrentRoomName() {
        return currentRoomBounds?.roomName;
    }
}
