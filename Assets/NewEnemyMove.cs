using UnityEngine;

public class NewEnemy : MonoBehaviour
{
    public float visionRange = 5f;      // 시야 범위 길이
    public float visionWidth = 1f;      // 시야 범위 폭
    public float hearingRange = 10f;    // 소리 인식 범위
    public float moveSpeed = 1f;
    public LayerMask playerLayer;       // 플레이어 레이어

    private Transform player;
    private PlayerMove playerScript;
    public GameObject visionObject;
    private bool isFacingRight = true;
    private bool isPlayerDetected = false;
    private bool attacking = false;
    private bool flipping = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<PlayerMove>();
        visionObject.transform.localPosition = new Vector3(3, 0, 0);
    }

    void Update()
    {
        if (player == null) return;

        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // 소리 범위 체크
        if (distanceToPlayer <= hearingRange && !attacking)
        {
            Debug.Log("플레이어가 소리 범위에 들어옴!");
            FollowPlayer();
        }

        // 이동 방향에 따라 시야 범위 회전
        UpdateVisionDirection(directionToPlayer);
    }

    void UpdateVisionDirection(Vector2 directionToPlayer)
    {
        if (directionToPlayer.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (directionToPlayer.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        if(!flipping){
            flipping = true;
            Debug.Log("Flip");
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;

            // 시야 범위 오브젝트도 회전
            visionObject.transform.localPosition = new Vector3(3, 0, 0);
            flipping = false;
        }
    }

    public void OnPlayerDetected()
    {
        isPlayerDetected = true;
        AttackPlayer();
    }

    public void OnPlayerLost()
    {
        isPlayerDetected = false;
        attacking = false;
    }

    private void FollowPlayer()
    {
        // 플레이어를 향해 이동합니다.
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        Debug.Log("이동");
    }

    private void AttackPlayer()
    {
        if (!isPlayerDetected) return;

        // 플레이어를 향해 이동합니다.
        attacking = true;
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        Debug.Log("공격");
    }
}
