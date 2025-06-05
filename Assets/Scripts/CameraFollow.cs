using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 마리오의 Transform
    public Vector3 offset;   // 카메라의 오프셋 값 (예: 왼쪽으로 이동)

    void LateUpdate()
    {
        // 카메라는 x 값만 따라가고, y와 z 값은 고정
        transform.position = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z);
    }
}
