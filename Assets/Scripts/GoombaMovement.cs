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
        Debug.Log("활성");
        animator.SetTrigger("Die");
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;


        //Destroy(gameObject, 0.5f);
        // 스프라이트를 찌그러진 걸로 바꿔
        GetComponent<SpriteRenderer>().sprite = squashedSprite;

        // 움직임 멈추고 콜라이더 끄기
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        // 아래로 떨어지도록 중력 증가
        rb.gravityScale = 5f;

        // 1.5초 후 파괴
        Destroy(gameObject, 1.5f);
    }


}