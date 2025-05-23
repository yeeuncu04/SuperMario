using UnityEngine;

public class MarioMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 10f;  // 점프 파워 추가
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

        // 이동 (Y축은 원래 Rigidbody의 속도를 유지)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 애니메이터에 speed 값 전달 (절댓값)
        animator.SetFloat("speed", Mathf.Abs(moveInput));

        // 방향 전환 (왼쪽 이동 시 flip)
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;  // 오른쪽 보고
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;   // 왼쪽 보고
        }

        // 점프 (조건 없이 스페이스 누를 때마다)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
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
            Destroy(other.gameObject);
            // 사운드 재생도 여기에!
        }
    }

}
