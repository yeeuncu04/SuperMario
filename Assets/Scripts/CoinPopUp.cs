using UnityEngine;

public class CoinPopUp : MonoBehaviour
{
    void Start()
    {
        // Rigidbody2D를 가져와서 rb 변수에 저장
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // 위쪽으로 튕기게 하기 (velocity → linearVelocity로 수정)
        rb.linearVelocity = new Vector2(0, 5f);

        // 잠시 뒤 사라지게
        Destroy(gameObject, 0.5f);
    }
}
