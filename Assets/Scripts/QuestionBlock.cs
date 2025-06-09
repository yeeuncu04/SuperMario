using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    private bool isHit = false;

    public AudioClip boxHitClip;         // ���� �Ҹ��� ������ ��
    public AudioSource sfxSource;        // ȿ���� �����
    public GameObject coinPrefab;        // Inspector���� ���� ������ ����

    void Start()
    {
        // ����� �ҽ� �������� ������ �߰�
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

        // �ڽ��� �ؿ��� �ƴ��� Ȯ�� (normal.y > 0.5f)
        if (collision.gameObject.CompareTag("Player") &&
            collision.contacts[0].normal.y > 0.5f)
        {
            isHit = true;

            // ���� ����
            if (coinPrefab != null)
            {
                Instantiate(coinPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }

            // ���� �Ҹ� ���
            if (boxHitClip != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(boxHitClip);
            }

            // ���� & ���� �ݿ�
            MarioMovement mario = collision.gameObject.GetComponent<MarioMovement>();
            if (mario != null)
            {
                mario.coinCount++;
                mario.AddScore(100);
            }

            // �ڽ� �ı� (��¦ �ʰ�)
            Destroy(gameObject, 0.2f);
        }
    }
}
