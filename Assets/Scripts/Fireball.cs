using UnityEngine;

public class Fireball : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("불이 마리오와 충돌!");

            MarioMovement mario = other.GetComponent<MarioMovement>();
            if (mario != null)
            {
                // 소리 재생
                if (mario.hitClip != null && mario.sfxSource != null)
                {
                    mario.sfxSource.PlayOneShot(mario.hitClip);
                }

                Debug.Log("마리오에 HandleFallDeath 호출!");
                mario.StartCoroutine(mario.HandleFallDeath());
            }

            Destroy(gameObject); // 불 사라지게
        }
    }
}
