using UnityEngine;
using TMPro;
using System.Collections;

public class MarioMovement : MonoBehaviour
{
    public int life = 3;
    public float moveSpeed = 5f;
    public float jumpPower = 10f;
    public Rigidbody2D rb;
    public Animator animator;
    public int coinCount = 0;
    public GameObject gameOverUI;
    public Vector3 respawnPosition;

    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;
    private bool isGrounded;

    private float moveInput;
    private SpriteRenderer spriteRenderer;

    public TextMeshProUGUI scoreValueText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI worldValueText;
    public TextMeshProUGUI timeValueText;

    private int score = 0;
    private float timeLeft = 300f;

    // 상태 제어 변수
    private bool isDead = false;
    private bool isGameOver = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        respawnPosition = transform.position;
    }

    void Update()
    {
        if (isDead || isGameOver) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        // 좌우 이동
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

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("speed", Mathf.Abs(moveInput));

        // 방향 반전
        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;

        // 시간 감소 및 UI 업데이트
        timeLeft -= Time.deltaTime;
        int intTime = Mathf.Max(0, Mathf.FloorToInt(timeLeft));

        scoreValueText.text = score.ToString("D6");
        coinText.text = coinCount.ToString("D2");
        worldValueText.text = "1-1";
        timeValueText.text = intTime.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead || isGameOver) return;

        if (other.CompareTag("Dead"))
        {
            StartCoroutine(HandleFallDeath());
        }
        else if (other.CompareTag("coin"))
        {
            coinCount++;
            score += 100;
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead || isGameOver) return;

        if (collision.gameObject.CompareTag("Goomba"))
        {
            StartCoroutine(HandleFallDeath());
        }
    }

    IEnumerator HandleFallDeath()
    {
        if (isDead || isGameOver) yield break;

        isDead = true;
        life--;
        Debug.Log("데미지! 남은 생명: " + life);

        if (life <= 0)
        {
            isGameOver = true;
            gameOverUI.SetActive(true);

            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Dynamic;
            GetComponent<Collider2D>().enabled = false;
            animator.SetFloat("speed", 0f);
            yield break;
        }

        // 낙사 연출
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.down * 5f, ForceMode2D.Impulse);
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(1f);

        // 리스폰
        transform.position = respawnPosition;
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = true;
        animator.SetFloat("speed", 0f);

        isDead = false;
    }
}
