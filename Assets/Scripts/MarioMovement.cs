using UnityEngine;
using TMPro;  // 💡 TextMeshPro를 위해 필요!

public class MarioMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 10f;
    public Rigidbody2D rb;
    public Animator animator;
    public int coinCount = 0;

    // GroundCheck 관련 변수
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;
    private bool isGrounded;

    private float moveInput;
    private SpriteRenderer spriteRenderer;

    // 💛 UI 관련 변수들
    public TextMeshProUGUI scoreValueText;    // 000000
    public TextMeshProUGUI coinText;          // x00
    public TextMeshProUGUI worldValueText;    // 1-1
    public TextMeshProUGUI timeValueText;     // 300

    private int score = 0;
    private float timeLeft = 300f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }

        // 키 입력 체크
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1;
        }
        else
        {
            moveInput = 0;
        }

        // 이동
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 애니메이터에 speed 값 전달
        animator.SetFloat("speed", Mathf.Abs(moveInput));

        // 방향 전환
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        // 💛 남은 시간 감소
        timeLeft -= Time.deltaTime;
        int intTime = Mathf.Max(0, Mathf.FloorToInt(timeLeft));

        // 💛 UI 업데이트
        scoreValueText.text = score.ToString("D6");       // 000000 형태
        coinText.text = coinCount.ToString("D2");   // x00 형태
        worldValueText.text = "1-1";
        timeValueText.text = intTime.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dead"))
        {
            Debug.Log("낙사!");
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("coin"))
        {
            coinCount++;
            score += 100;  // 💛 점수도 추가
            Destroy(other.gameObject);
            // 사운드도 여기에 추가 가능!
        }
    }
}
