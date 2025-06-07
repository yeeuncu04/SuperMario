using System.Collections;
using UnityEngine;

public class MarioController : MonoBehaviour
{
    public int life = 3;
    public Vector3 respawnPosition;

    public GameObject gameOverUI;  // ���ӿ��� UI ����

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        respawnPosition = transform.position;
        gameOverUI.SetActive(false);  // ������ �� ���ӿ��� UI�� ������
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGameOver) return;  // ���ӿ��� �� �浹 ����

        if (collision.gameObject.CompareTag("Goomba"))
        {
            HandleHitByGoomba();
        }
    }

    void HandleHitByGoomba()
    {
        life--;
        if (life <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(KnockbackAndRespawn());
        }
    }

    IEnumerator KnockbackAndRespawn()
    {
        // 1. ��� �������� �ִϸ��̼� ��� �ٷ� �Ʒ��� ����߸���
        float fallDistance = 3f; // ����� ��� ���������� y�� ����
        Vector3 fallPos = transform.position + Vector3.down * fallDistance;

        float elapsed = 0f;
        float duration = 0.5f;  // �������� �ð�

        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, fallPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = fallPos;

        // 2. ��� ��ٷȴٰ� ������ ��ġ�� �̵�
        yield return new WaitForSeconds(0.5f);

        transform.position = respawnPosition;
        animator.SetFloat("Speed", 0f);
    }


    void GameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
        rb.linearVelocity = Vector2.zero;
        // ������ ���� ������Ʈ ��Ȱ��ȭ �Ǵ� �߰� ó�� ����
        Debug.Log("���ӿ���!");
    }
}
