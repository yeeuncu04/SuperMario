using UnityEngine;

public class KoopaController : MonoBehaviour
{
    public float minMoveSpeed = 0.5f;
    public float maxMoveSpeed = 2f;
    public float shellMoveSpeed = 8f;
    public Sprite shellSprite;

    public bool move = true;
    public Transform leftSpot;
    public Transform rightSpot;

    private bool isShell = false;
    public bool isShellMoving = false;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float moveSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

        if (Random.value > 0.5f)
            Flip();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EnterShell();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartShell();
        }
    }


    void FixedUpdate()
    {
        if (!isShell && move)
        {
            Vector2 dir = new Vector2(IsFacingLeft() ? -moveSpeed : moveSpeed, rb.linearVelocity.y);
            rb.position += dir * Time.deltaTime;

            if (!IsFacingLeft() && rb.position.x >= rightSpot.position.x)
            {
                Flip();
            }
            else if (IsFacingLeft() && rb.position.x <= leftSpot.position.x)
            {
                Flip();
            }
        }
        else if (!isShellMoving)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            Vector2 dir = new Vector2(IsFacingLeft() ? -shellMoveSpeed : shellMoveSpeed, rb.linearVelocity.y);
            rb.position += dir * Time.deltaTime;
        }
    }


    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    bool IsFacingLeft()
    {
        return transform.localScale.x >= 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("Player"))
        {
            MarioMovement mario = other.GetComponent<MarioMovement>();
            if (mario != null)
            {
                Vector2 normal = collision.contacts[0].normal;
                float deg = Vector2.Angle(Vector2.down, normal);
                bool hitFromAbove = deg < 45;

                if (hitFromAbove && !isShell)
                {
                    // 마리오가 쿠파를 밟으면 등껍질로 변함
                    EnterShell();
                    mario.rb.linearVelocity = new Vector2(mario.rb.linearVelocity.x, 8f);
                }
                else if (hitFromAbove && isShell && isShellMoving)
                {
                    // 움직이는 등껍질 밟으면 멈춤
                    StopShell();
                    mario.rb.linearVelocity = new Vector2(mario.rb.linearVelocity.x, 8f);
                }
                else if (hitFromAbove && isShell && !isShellMoving)
                {
                    // 정지된 등껍질 위를 밟으면 튕기기만
                    mario.rb.linearVelocity = new Vector2(mario.rb.linearVelocity.x, 8f);
                }
                else if (!hitFromAbove && isShell && !isShellMoving)
                {
                    // 플레이어가 왼쪽에서 충돌...
                    if (normal.x > 0.5f && IsFacingLeft())
                    {
                        Flip();
                    }
                    else if (normal.x < 0.5f && !IsFacingLeft())
                    {
                        Flip();
                    }

                    StartShell();
                }
                else if (!hitFromAbove && !isShell)
                {
                    // 그 외의 경우 (옆에서 닿았거나 움직이는 등껍질에 닿으면) → 마리오 죽음
                    mario.Die();
                }
            }
        }

        // 움직이는 등껍질이 굼바에 닿으면 굼바 제거
        if (other.CompareTag("Goomba") && isShell && isShellMoving)
        {
            GoombaMovement goomba = other.GetComponent<GoombaMovement>();
            if (goomba != null)
            {
                goomba.OnStomped();
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }


    void EnterShell()
    {
        isShell = true;
        isShellMoving = false;
        spriteRenderer.sprite = shellSprite;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("등껍질로 변신!");
    }

    void StartShell()
    {
        isShellMoving = true;
        float dir = IsFacingLeft() ? -2f : 2f;
        rb.linearVelocity = new Vector2(dir * shellMoveSpeed, rb.linearVelocity.y);
        Debug.Log("등껍질 굴러가기 시작!");
    }

    void StopShell()
    {
        isShellMoving = false;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("등껍질 멈춤!");
    }
}
