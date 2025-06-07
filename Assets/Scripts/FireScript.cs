using UnityEngine;

public class FireScript : MonoBehaviour
{
    public float speed = 5f; // �� �̵� �ӵ�

    void Update()
    {
        // �������� ��� �̵�
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ������ ���̱�
            Destroy(other.gameObject); // �Ǵ� Die() �Լ� ȣ��
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        // ī�޶� ȭ�� ������ ������ ����
        Destroy(gameObject);
    }
}
