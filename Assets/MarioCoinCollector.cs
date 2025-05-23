using UnityEngine;

public class MarioCoinCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("coin"))
        {
            Destroy(collision.gameObject); // 코인 사라짐!
        }
    }
}
