using UnityEngine;

public class CoinPopUp : MonoBehaviour
{
    void Start()
    {
        // Rigidbody2D�� �����ͼ� rb ������ ����
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // �������� ƨ��� �ϱ� (velocity �� linearVelocity�� ����)
        rb.linearVelocity = new Vector2(0, 5f);

        // ��� �� �������
        Destroy(gameObject, 0.5f);
    }
}
