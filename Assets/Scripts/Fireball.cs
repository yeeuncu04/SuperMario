using UnityEngine;

public class Fireball : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("���� �������� �浹!");

            MarioMovement mario = other.GetComponent<MarioMovement>();
            if (mario != null)
            {
                // �Ҹ� ���
                if (mario.hitClip != null && mario.sfxSource != null)
                {
                    mario.sfxSource.PlayOneShot(mario.hitClip);
                }

                Debug.Log("�������� HandleFallDeath ȣ��!");
                mario.StartCoroutine(mario.HandleFallDeath());
            }

            Destroy(gameObject); // �� �������
        }
    }
}
