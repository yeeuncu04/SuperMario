using UnityEngine;

public class GoombaMovement : MonoBehaviour
{
    public float speed = 0.01f;
    private bool movingRight = true;
    private Rigidbody2D rb;
    private Animator animator;

    public Transform leftSpot;
    public Transform rightSpot;
    public Sprite squashedSprite;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        rb.linearVelocity = new Vector2(movingRight ? speed : -speed, rb.linearVelocity.y);

        if (movingRight && rb.position.x >= rightSpot.position.x)
        {
            Flip();
        }
        else if (!movingRight && rb.position.x <= leftSpot.position.x)
        {
            Flip();
        }

        //RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 1f);
        //if (groundInfo.collider == false)
        //{
        //    Flip();
        //}


        //RaycastHit2D wallInfo = Physics2D.Raycast(transform.position, movingRight ? Vector2.right : Vector2.left, 0.1f);
        //if (wallInfo.collider != null && !wallInfo.collider.CompareTag("Player"))
        //{
        //    Flip();
        //}

        animator.SetFloat("Speed", Mathf.Abs(speed));
    }

    void Flip()
    {
        movingRight = !movingRight;
        //Vector3 localScale = transform.localScale; 
        //transform.localScale = localScale;
    }


    public void OnStomped()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("굼바가 밟혔어요!");

        // Animator 끄기
        Animator animator = GetComponent<Animator>();
        if (animator != null) animator.enabled = false;

        // SpriteRenderer에 squashedSprite 적용
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && squashedSprite != null)
        {
            sr.sprite = squashedSprite;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer나 squashedSprite가 null입니다!");
        }

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 5f;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 0.5f);
    }




}