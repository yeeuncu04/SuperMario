using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // �������� Transform
    public Vector3 offset;   // ī�޶��� ������ ��

    public float minX;       // ī�޶� �� �� �ִ� �ּ� X��
    public float maxX;       // ī�޶� �� �� �ִ� �ִ� X��

    public string playerTag = "Player"; // �±׷� �÷��̾� ��Ž��

    void LateUpdate()
    {
        // �÷��̾ ������ٸ� �±׷� �ٽ� ã��
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag(playerTag);
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
            else
            {
                return; // ���� ������ �� ������ �� �������� ����
            }
        }

        // ��ġ ���
        float targetX = player.position.x + offset.x;

        // X �� ����
        float clampedX = Mathf.Clamp(targetX, minX, maxX);

        // ī�޶� �̵�
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
