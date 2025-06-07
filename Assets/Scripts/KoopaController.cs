using UnityEngine;

public class KoopaController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float directionChangeTime = 10f; // 10초마다 방향 전환
    private float direction = -1f;
    private float timer;

    void Start()
    {
        timer = directionChangeTime;
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            direction *= -1f;
            Flip();
            timer = directionChangeTime;
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    // 🔽 마리오랑 부딪히면 제거
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
