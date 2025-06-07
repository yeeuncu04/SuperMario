using UnityEngine;

public class MarioFlagTrigger : MonoBehaviour
{
    public Transform flagBottomPoint;     // 마리오가 내려갈 위치 기준점
    public float slideSpeed = 2f;         // 슬라이딩 속도
    public float walkSpeed = 2f;          // 오른쪽 자동 걷기 속도

    public Transform flag;                // 깃발 오브젝트
    public float flagLowerYPosition;      // 깃발이 내려갈 최종 Y 좌표

    public float walkDuration = 2.5f;     // 걷는 시간 (성으로 들어가기까지 시간)
    public GameObject gameOverUI;         // 게임오버 UI 오브젝트

    private bool isTouchingFlag = false;
    private bool isSlidingDown = false;
    private bool isWalkingToGoal = false;

    private float walkTimer = 0f;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flag"))
        {
            isTouchingFlag = true;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            animator.SetFloat("Speed", 0);
        }
    }

    void Update()
    {
        if (isTouchingFlag && !isSlidingDown)
        {
            // 마리오 슬라이딩
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(transform.position.x, flagBottomPoint.position.y),
                slideSpeed * Time.deltaTime
            );

            // 깃발 내려감
            if (flag != null)
            {
                flag.position = Vector2.MoveTowards(
                    flag.position,
                    new Vector2(flag.position.x, flagLowerYPosition),
                    slideSpeed * Time.deltaTime
                );
            }

            // 슬라이딩 끝
            if (Mathf.Abs(transform.position.y - flagBottomPoint.position.y) < 0.05f)
            {
                isSlidingDown = true;
                isTouchingFlag = false;
                Invoke("StartWalkingToGoal", 0.5f);
            }
        }

        if (isWalkingToGoal)
        {
            transform.Translate(Vector2.right * walkSpeed * Time.deltaTime);
            animator.SetFloat("Speed", 1);

            walkTimer -= Time.deltaTime;
            if (walkTimer <= 0f)
            {
                isWalkingToGoal = false;
                animator.SetFloat("Speed", 0);

                if (gameOverUI != null)
                    gameOverUI.SetActive(true);
            }
        }
    }

    void StartWalkingToGoal()
    {
        isWalkingToGoal = true;
        walkTimer = walkDuration;
    }
}
