using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    private bool isHit = false;

    public AudioClip boxHitClip;         // 코인 소리로 설정할 것
    public AudioSource sfxSource;        // 효과음 재생용
    public GameObject coinPrefab;        // Inspector에서 코인 프리팹 연결

    void Start()
    {
        // 오디오 소스 가져오기 없으면 추가
        if (sfxSource == null)
        {
            sfxSource = GetComponent<AudioSource>();
            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHit) return;

        // 박스를 밑에서 쳤는지 확인 (normal.y > 0.5f)
        if (collision.gameObject.CompareTag("Player") &&
            collision.contacts[0].normal.y > 0.5f)
        {
            isHit = true;

            // 코인 생성
            if (coinPrefab != null)
            {
                Instantiate(coinPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }

            // 코인 소리 재생
            if (boxHitClip != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(boxHitClip);
            }

            // 점수 & 코인 반영
            MarioMovement mario = collision.gameObject.GetComponent<MarioMovement>();
            if (mario != null)
            {
                mario.coinCount++;
                mario.AddScore(100);
            }

            // 박스 파괴 (살짝 늦게)
            Destroy(gameObject, 0.2f);
        }
    }
}
