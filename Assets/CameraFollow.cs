using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // �������� Transform
    public Vector3 offset;   // ī�޶��� ������ �� (��: �������� �̵�)

    void LateUpdate()
    {
        // ī�޶�� x ���� ���󰡰�, y�� z ���� ����
        transform.position = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z);
    }
}
