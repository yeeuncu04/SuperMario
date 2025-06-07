using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 마리오의 Transform
    public Vector3 offset;   // 카메라의 오프셋 값

    public float minX;       // 카메라가 갈 수 있는 최소 X값
    public float maxX;       // 카메라가 갈 수 있는 최대 X값

    void LateUpdate()
    {
        float targetX = player.position.x + offset.x;

        // 카메라 X 위치를 minX ~ maxX 범위로 제한
        float clampedX = Mathf.Clamp(targetX, minX, maxX);

        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
