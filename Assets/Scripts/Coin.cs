using UnityEngine;

public class Coin : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.5f); // 위로 튀면서 사라지게
    }

    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * 2; // 위로 튀는 효과
    }
}
