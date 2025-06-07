using UnityEngine;

public class GoombaMovement : MonoBehaviour
{
    public float speed = 0.01f;     
    private bool movingRight = true;  
    private Rigidbody2D rb;
    private Animator animator;

    public Transform groundDetection;  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
     
        rb.linearVelocity = new Vector2(movingRight ? speed : -speed, rb.linearVelocity.y);

       
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 1f);
        if (groundInfo.collider == false)
        {
            Flip();
        }

       
        RaycastHit2D wallInfo = Physics2D.Raycast(transform.position, movingRight ? Vector2.right : Vector2.left, 0.1f);
        if (wallInfo.collider != null && !wallInfo.collider.CompareTag("Player"))
        {
            Flip();
        }

        animator.SetFloat("Speed", Mathf.Abs(speed));
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 localScale = transform.localScale; 
        transform.localScale = localScale;
    }


    public void OnStomped()
    {
    
        animator.SetTrigger("Die");
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

 
        Destroy(gameObject, 0.5f);
    }
}