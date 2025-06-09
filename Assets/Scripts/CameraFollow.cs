using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 마리오의 Transform
    public Vector3 offset;   // 카메라의 오프셋 값

    public float minX;       // 카메라가 갈 수 있는 최소 X값
    public float maxX;       // 카메라가 갈 수 있는 최대 X값

    public string playerTag = "Player"; // 태그로 플레이어 재탐색

    void LateUpdate()
    {
        // 플레이어가 사라졌다면 태그로 다시 찾기
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag(playerTag);
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
            else
            {
                return; // 아직 리스폰 안 됐으면 더 진행하지 않음
            }
        }

        // 위치 계산
        float targetX = player.position.x + offset.x;

        // X 값 제한
        float clampedX = Mathf.Clamp(targetX, minX, maxX);

        // 카메라 이동
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
