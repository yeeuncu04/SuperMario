using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    private bool isHit = false;

    public GameObject coinPrefab; // Inspector에서 프리팹 연결

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHit) return;

        // 충돌한 방향이 아래쪽(=박스를 밑에서 친 경우)
        if (collision.gameObject.CompareTag("Player") &&
            collision.contacts[0].normal.y > 0.5f)
        {
            isHit = true;

            // 코인 생성
            if (coinPrefab != null)
            {
                Instantiate(coinPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                
            }

            // 박스 파괴
            Destroy(gameObject);
        }

    }
}
