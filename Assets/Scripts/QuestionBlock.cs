using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    private bool isHit = false;

    public GameObject coinPrefab; // Inspector���� ������ ����

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHit) return;

        // �浹�� ������ �Ʒ���(=�ڽ��� �ؿ��� ģ ���)
        if (collision.gameObject.CompareTag("Player") &&
            collision.contacts[0].normal.y > 0.5f)
        {
            isHit = true;

            // ���� ����
            if (coinPrefab != null)
            {
                Instantiate(coinPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                
            }

            // �ڽ� �ı�
            Destroy(gameObject);
        }

    }
}
