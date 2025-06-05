using UnityEngine;

public class GoombaMovement : MonoBehaviour
{
    public float speed = 0.01f;          // �̵� �ӵ�
    private bool movingRight = true;  // �̵� ����
    private Rigidbody2D rb;
    private Animator animator;

    public Transform groundDetection;  // �ٴ� üũ�� ��ġ (�� ��)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ��� ���� �ӵ��� �̵�
        rb.linearVelocity = new Vector2(movingRight ? speed : -speed, rb.linearVelocity.y);

        // �ٴ� ������ ����ĳ��Ʈ (�߹ؿ� ���� ������ ���� ��ȯ)
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 1f);
        if (groundInfo.collider == false)
        {
            Flip();
        }

        // �տ� �� ���� (�÷��̾ ��� �� ����, ���� �ε����� ���� ��ȯ)
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
        localScale.x *= -1; // ���⿡ ���� ��������Ʈ �¿� ����
        transform.localScale = localScale;
    }

    // �÷��̾ ����� �� ȣ���� �Լ� (�ݶ��̴� Ʈ���ſ��� ȣ��)
    public void OnStomped()
    {
        // �״� �ִϸ��̼� �����ϰų� �ٷ� ����
        animator.SetTrigger("Die");
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        // 0.5�� �� ������Ʈ ����
        Destroy(gameObject, 0.5f);
    }
}
