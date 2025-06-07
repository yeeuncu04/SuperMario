using UnityEngine;

public class Coinup : MonoBehaviour
{
    public float jumpHeight = 1f;
    public float disappearTime = 0.3f;

    void Start()
    {
        // 위로 살짝 튀기기
        transform.Translate(Vector3.up * jumpHeight);
        // 일정 시간 후 사라짐
        Destroy(gameObject, disappearTime);
    }
}
