using UnityEngine;

public class MarioFlagTrigger : MonoBehaviour
{
    public Transform flagBottomPoint;     // �������� ������ ��ġ ������
    public float slideSpeed = 2f;         // �����̵� �ӵ�
    public float walkSpeed = 2f;          // ������ �ڵ� �ȱ� �ӵ�

    public Transform flag;                // ��� ������Ʈ
    public float flagLowerYPosition;      // ����� ������ ���� Y ��ǥ

    public float walkDuration = 2.5f;     // �ȴ� �ð� (������ ������� �ð�)
    public GameObject gameOverUI;         // ���ӿ��� UI ������Ʈ

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
            // ������ �����̵�
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(transform.position.x, flagBottomPoint.position.y),
                slideSpeed * Time.deltaTime
            );

            // ��� ������
            if (flag != null)
            {
                flag.position = Vector2.MoveTowards(
                    flag.position,
                    new Vector2(flag.position.x, flagLowerYPosition),
                    slideSpeed * Time.deltaTime
                );
            }

            // �����̵� ��
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
