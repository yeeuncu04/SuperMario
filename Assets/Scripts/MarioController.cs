using System.Collections;
using UnityEngine;

public class MarioController : MonoBehaviour
{
    public int life = 3;
    public Vector3 respawnPosition;

    public GameObject gameOverUI;  // 게임오버 UI 연결

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        respawnPosition = transform.position;
        gameOverUI.SetActive(false);  // 시작할 때 게임오버 UI는 꺼놓기
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGameOver) return;  // 게임오버 시 충돌 무시

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
        // 1. 잠깐 떨어지는 애니메이션 대신 바로 아래로 떨어뜨리기
        float fallDistance = 3f; // 충분히 깊게 떨어지도록 y값 조절
        Vector3 fallPos = transform.position + Vector3.down * fallDistance;

        float elapsed = 0f;
        float duration = 0.5f;  // 떨어지는 시간

        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, fallPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = fallPos;

        // 2. 잠시 기다렸다가 리스폰 위치로 이동
        yield return new WaitForSeconds(0.5f);

        transform.position = respawnPosition;
        animator.SetFloat("Speed", 0f);
    }


    void GameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
        rb.linearVelocity = Vector2.zero;
        // 움직임 관련 컴포넌트 비활성화 또는 추가 처리 가능
        Debug.Log("게임오버!");
    }
}
