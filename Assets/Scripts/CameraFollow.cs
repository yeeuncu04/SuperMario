using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // �������� Transform
    public Vector3 offset;   // ī�޶��� ������ ��

    public float minX;       // ī�޶� �� �� �ִ� �ּ� X��
    public float maxX;       // ī�޶� �� �� �ִ� �ִ� X��

    void LateUpdate()
    {
        float targetX = player.position.x + offset.x;

        // ī�޶� X ��ġ�� minX ~ maxX ������ ����
        float clampedX = Mathf.Clamp(targetX, minX, maxX);

        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
