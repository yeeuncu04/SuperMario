using UnityEngine;

public class FireScript : MonoBehaviour
{
    public float speed = 5f; // 불 이동 속도

    void Update()
    {
        // 왼쪽으로 계속 이동
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 마리오 죽이기
            Destroy(other.gameObject); // 또는 Die() 함수 호출
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        // 카메라 화면 밖으로 나가면 삭제
        Destroy(gameObject);
    }
}
