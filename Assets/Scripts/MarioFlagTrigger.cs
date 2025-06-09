using UnityEngine;

public class MarioFlagTrigger : MonoBehaviour
{
    public Transform flagBottomPoint;
    public float slideSpeed = 2f;
    public float walkSpeed = 2f;

    public Transform flag;
    public float flagLowerYPosition;

    public float walkDuration = 2.5f;
    public GameObject gameOverUI;

    private bool isTouchingFlag = false;
    private bool isSlidingDown = false;
    private bool isWalkingToGoal = false;

    private float walkTimer = 0f;
    private Rigidbody2D rb;
    private Animator animator;

    public AudioClip goalMusic;
    public AudioSource sfxSource; // 인스펙터에서 연결하도록 변경

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // audioSource = GetComponent<AudioSource>(); // 제거
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flag"))
        {
            isTouchingFlag = true;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            animator.SetFloat("Speed", 0);

            // 소리 재생
            if (goalMusic != null && sfxSource != null)
            {
                sfxSource.Stop();
                sfxSource.clip = goalMusic;
                sfxSource.Play();
            }
        }
    }

    void Update()
    {
        if (isTouchingFlag && !isSlidingDown)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(transform.position.x, flagBottomPoint.position.y),
                slideSpeed * Time.deltaTime
            );

            if (flag != null)
            {
                flag.position = Vector2.MoveTowards(
                    flag.position,
                    new Vector2(flag.position.x, flagLowerYPosition),
                    slideSpeed * Time.deltaTime
                );
            }

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
