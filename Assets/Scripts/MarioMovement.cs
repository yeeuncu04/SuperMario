using UnityEngine;
using TMPro;
using System.Collections;

public class MarioMovement : MonoBehaviour
{
    private int jumpCount = 0;
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
    public bool isGrounded;
   

    private float moveInput;
    private SpriteRenderer spriteRenderer;

    public TextMeshProUGUI scoreValueText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI worldValueText;
    public TextMeshProUGUI timeValueText;

    [Header("사운드 클립들")]
    public AudioClip jumpClip;
    public AudioClip coinClip;
    public AudioClip hitClip;
    public AudioClip gameOverClip;
    public AudioClip boxClip;
    public AudioClip bgmClip;

    [Header("사운드 소스")]
    public AudioSource sfxSource;     // 효과음 전용
    public AudioSource bgmSource;     // 배경음 전용

    private int score = 0;
    private float timeLeft = 300f;

    public bool isDead = false;
    private bool isGameOver = false;
    private bool isCoinCollected = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        respawnPosition = transform.position;

        // 배경음 시작
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    void Update()
    {
        //Debug.Log(jumpCount);
        if (isDead || isGameOver) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (isGrounded) jumpCount = 0;

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 1)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // 점프 전에 수직 속도 초기화
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;

            if (jumpClip != null && sfxSource != null)
                sfxSource.PlayOneShot(jumpClip);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1;
        else if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1;
        else moveInput = 0;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("speed", Mathf.Abs(moveInput));

        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

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

        // 코인 먹었을 때
        if (other.CompareTag("coin") && !isCoinCollected)
        {
            isCoinCollected = true;

            coinCount++;
            AddScore(100);
            if (coinClip != null && sfxSource != null)
                sfxSource.PlayOneShot(coinClip);

            Destroy(other.gameObject);

            StartCoroutine(ResetCoinCollectedFlag());
        }

        // 낙사 영역 (Fall) 들어갔을 때
        else if (other.CompareTag("Dead"))
        { 
                if (isDead) return;

            if (hitClip != null && sfxSource != null)
                sfxSource.PlayOneShot(hitClip);

            StartCoroutine(HandleFallDeath());
        }

        // 박스 부딪힘 예시 (원하는 곳에 박스 태그 사용)
        else if (other.CompareTag("QuestionBlock"))
        {
            if (boxClip != null && sfxSource != null)
                sfxSource.PlayOneShot(boxClip);
        }

        // whatIsGround 레이어 마스크 체크 (필요하다면)
        if (((1 << other.gameObject.layer) & whatIsGround) != 0)
        {
            jumpCount = 0;
        }

        if (isDead || isGameOver) return;

        if (other.CompareTag("head"))
        {
            if (transform.position.y > other.transform.position.y)
            {
                Debug.Log("헤드 트리거 감지");

                GoombaMovement goomba = other.GetComponent<GoombaMovement>();
                if (goomba == null && other.transform.parent != null)
                {
                    goomba = other.transform.parent.GetComponent<GoombaMovement>();
                }

                Debug.Log("GoombaMovement 컴포넌트 가져오기 시도: " + (goomba != null));

                if (goomba != null)
                {
                    goomba.OnStomped();
                    Debug.Log("데미지! 남은 생명: " + life);
                    //jumpCount = 0;
                }
                else
                {
                    Debug.LogWarning("GoombaMovement 컴포넌트를 찾지 못함!");
                }
            }
            else
            {
                if (hitClip != null && sfxSource != null)
                    sfxSource.PlayOneShot(hitClip);

                StartCoroutine(HandleFallDeath());
            }
        }

    }

    IEnumerator ResetCoinCollectedFlag()
    {
        yield return new WaitForSeconds(0.1f);
        isCoinCollected = false;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (((1 << collision.gameObject.layer) & whatIsGround) != 0)
    //    {
    //        jumpCount = 0;
    //    }

    //    if (isDead || isGameOver) return;

    //    if (collision.gameObject.CompareTag("head"))
    //    {
    //        if (collision.contacts[0].normal.y > 0.5f)
    //        {
    //            // 마리오가 위에서 밟음
    //            //rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower * 0.8f);
    //            Debug.Log("헤드");
    //            GoombaMovement goomba = collision.gameObject.GetComponent<GoombaMovement>();
    //            if (goomba != null)
    //            {
    //                goomba.OnStomped();
    //                Debug.Log("데미지! 남은 생명: " + life);
    //            }
    //        }
    //        else
    //        {
    //            // 마리오가 데미지 받음
    //            if (hitClip != null && sfxSource != null)
    //                sfxSource.PlayOneShot(hitClip);

    //            StartCoroutine(HandleFallDeath());
    //        }
    //    }
    //}



    public IEnumerator HandleFallDeath()
    {
        if (isDead || isGameOver) yield break;

        isDead = true;
        life--;
        Debug.Log("데미지! 남은 생명: " + life);

        if (life <= 0)
        {
            isGameOver = true;

            int bonus = Mathf.FloorToInt(timeLeft) * 50;
            score += bonus;
            timeLeft = 0f;

            scoreValueText.text = score.ToString("D6");
            timeValueText.text = "0";

            gameOverUI.SetActive(true);

            rb.linearVelocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
            animator.SetFloat("speed", 0f);

            if (gameOverClip != null && sfxSource != null)
                sfxSource.PlayOneShot(gameOverClip);

            Debug.Log("게임오버! 최종 점수: " + score);
            yield break;
        }

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.down * 5f, ForceMode2D.Impulse);
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(1f);

        // 리스폰
        transform.position = respawnPosition;

        // Rigidbody 상태 초기화
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // <- 이것도 꼭 해줘야 움직임이 복구돼

        // 콜라이더와 애니메이션 복구
        GetComponent<Collider2D>().enabled = true;
        animator.SetFloat("speed", 0f);

        // 상태 복구
        isDead = false;

    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreValueText.text = score.ToString("D6");
        coinText.text = coinCount.ToString("D2");
    }
}
