using UnityEngine;

public class Coinup : MonoBehaviour
{
    public float jumpHeight = 1f;
    public float disappearTime = 0.3f;

    void Start()
    {
        // ���� ��¦ Ƣ���
        transform.Translate(Vector3.up * jumpHeight);
        // ���� �ð� �� �����
        Destroy(gameObject, disappearTime);
    }
}
