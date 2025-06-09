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
                Debug.Log("마리오에 HandleFallDeath 호출!");
                mario.StartCoroutine(mario.HandleFallDeath());
            }

            // 불은 마리오랑 부딪힌 후 사라지도록 설정
            Destroy(gameObject);
        }
        //else if (!other.isTrigger) // 벽 등과 부딪히면 불 삭제
        //{
          //  Destroy(gameObject);
        //}
    }
}
