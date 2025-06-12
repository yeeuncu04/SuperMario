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

    // 깃발(레벨 클리어) 관련 변수 추가
    private bool isLevelClear = false;
    public Transform flagPole;

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
        if (isDead || isGameOver || isLevelClear) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) timeLeft = 0;

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
        if (isDead || isGameOver || isLevelClear) return;

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

        if (other.CompareTag("head"))
        {
            if (transform.position.y > other.transform.position.y)
            {
                GoombaMovement goomba = other.GetComponent<GoombaMovement>();
                if (goomba == null && other.transform.parent != null)
                {
                    goomba = other.transform.parent.GetComponent<GoombaMovement>();
                }

                if (goomba != null)
                {
                    goomba.OnStomped();
                    AddScore(200);
                }

            }
            else
            {
                if (hitClip != null && sfxSource != null)
                    sfxSource.PlayOneShot(hitClip);

                StartCoroutine(HandleFallDeath());
            }
        }

        // 깃발 닿으면 레벨 클리어 시퀀스 시작
        if (other.CompareTag("Flag"))
        {
            isLevelClear = true;
            isGameOver = true; 
            StartCoroutine(LevelClearSequence());
        }
    }
    // 입력 막기 위해 같이 설정
    IEnumerator ResetCoinCollectedFlag()
    {
        yield return new WaitForSeconds(0.1f);
        isCoinCollected = false;
    }

    public IEnumerator HandleFallDeath()
    {
        if (isDead || isGameOver) yield break;

        isDead = true;
        life--;

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

            yield break;
        }

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.down * 5f, ForceMode2D.Impulse);
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(1f);


        transform.position = respawnPosition;

      
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

       
        GetComponent<Collider2D>().enabled = true;
        animator.SetFloat("speed", 0f);

        isDead = false;
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreValueText.text = score.ToString("D6");
        coinText.text = coinCount.ToString("D2");
    }

    // 레벨 클리어 후 자동 이동, 점수 계산, 시간 0 처리
    IEnumerator LevelClearSequence()
    {
        // 깃발로 이동 (속도 조절)
        float targetX = flagPole.position.x;
        while (transform.position.x < targetX)
        {
            rb.linearVelocity = new Vector2(moveSpeed * 0.5f, rb.linearVelocity.y);
            animator.SetFloat("speed", 0.5f);
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        animator.SetFloat("speed", 0f);

        // 점수 계산 코루틴 호출
        yield return StartCoroutine(ScoreCalculation());

        // 시간 0으로 만들기
        timeLeft = 0f;
        timeValueText.text = "0";
        yield return null;
        // 여기서 성 입장 애니메이션, 씬 전환 등 추가 가능
    }

    IEnumerator ScoreCalculation()
    {
        // 예: 코인, 적 처치 점수 외 시간 점수 더하기
        int timeBonus = Mathf.FloorToInt(timeLeft) * 50;
        int totalScore = score + timeBonus;

        // 점수 UI 애니메이션 효과 간단히 구현 (예시)
        int displayedScore = score;
        while (displayedScore < totalScore)
        {
            displayedScore += 100;
            if (displayedScore > totalScore) displayedScore = totalScore;

            scoreValueText.text = displayedScore.ToString("D6");
            yield return new WaitForSeconds(0.05f);
        }

        // 최종 점수 저장
        score = totalScore;
    }
    public void Die()
    {
        if (!isDead && !isGameOver)
        {
            StartCoroutine(HandleFallDeath());
        }
    }

}
