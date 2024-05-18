using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour {
    [SerializeField] float smoothing = 0.2f;
    public float maxVerticalOffset = 5f; // 카메라가 위로 이동하는 최대 오프셋
    public float minY = -3f; // 카메라가 내려갈 수 있는 최소 y 좌표
    public float maxY = 30f;

    private bool isFollowing; // 카메라가 따라가는 중인지 여부를 나타내는 플래그

    private Transform player; // 플레이어의 Transform
    public Vector3 OriginalPosition;

    private void Awake() {
        DontDestroyOnLoad(gameObject); // 다른 맵에서 없어지지 않게 해줌
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
        if (player != null) {
            // 캐릭터와 카메라의 거리 계산
            float verticalOffset = player.position.y - transform.position.y;
            float horizontalOffset = player.position.x - transform.position.x;

            // 캐릭터가 일정 오프셋 이상 위로 이동했을 때만 카메라를 따라감
            if (Mathf.Abs(verticalOffset) > maxVerticalOffset) {
                isFollowing = true;
            }

            if (horizontalOffset < 0) {
                transform.Translate(Vector3.left * Mathf.Abs(horizontalOffset), Space.World);
            }
            // 캐릭터가 카메라 오른쪽에 있으면 카메라도 오른쪽으로 이동
            else if (horizontalOffset > 0) {
                transform.Translate(Vector3.right * Mathf.Abs(horizontalOffset), Space.World);
            }

            // 카메라가 따라가는 중일 때만 캐릭터를 따라감
            if (isFollowing) {
                // 카메라 이동
                Vector3 targetPosition = new Vector3(transform.position.x, player.position.y - maxVerticalOffset, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);

                // 카메라가 minY 보다 아래로 내려가지 않도록 함
                if (transform.position.y > maxY) {
                    transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
                    isFollowing = false; // 카메라가 maxY 이상 올라갔을 때 따라가지 않도록 설정
                }
                if (transform.position.y < minY) {
                    transform.position = new Vector3(transform.position.x, minY, transform.position.z);
                    isFollowing = false; // 카메라가 minY 이하로 내려갔을 때 따라가지 않도록 설정
                }
            }
        }
        OriginalPosition = transform.position;
    }
}
