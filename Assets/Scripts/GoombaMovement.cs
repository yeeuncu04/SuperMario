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

        animator.SetFloat("Speed", Mathf.Abs(speed));
    }

    void Flip()
    {
        movingRight = !movingRight;
    }


    public void OnStomped()
    {
        if (isDead) return;
        isDead = true;

        Animator animator = GetComponent<Animator>();
        if (animator != null) animator.enabled = false;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && squashedSprite != null)
        {
            sr.sprite = squashedSprite;
        }

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 5f;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 0.5f);
    }




}