using UnityEngine;

public class Coin : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.5f); // ���� Ƣ�鼭 �������
    }

    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * 2; // ���� Ƣ�� ȿ��
    }
}
