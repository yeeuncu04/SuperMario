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
                Debug.Log("�������� HandleFallDeath ȣ��!");
                mario.StartCoroutine(mario.HandleFallDeath());
            }

            // ���� �������� �ε��� �� ��������� ����
            Destroy(gameObject);
        }
        //else if (!other.isTrigger) // �� ��� �ε����� �� ����
        //{
          //  Destroy(gameObject);
        //}
    }
}
