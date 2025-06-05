using UnityEngine;

public class CoinBox : MonoBehaviour
{
    public GameObject coinPrefab; // 코인 프리팹
    private bool isUsed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 마리오 머리의 충돌을 감지하려면 충돌 방향이 아래에서 올라온 경우만 체크
        if (!isUsed && collision.contacts[0].normal.y > 0.5f)
        {
            if (collision.collider.CompareTag("Player")) // 또는 마리오 태그
            {
                SpawnCoin();
                isUsed = true;
                ChangeToUsedBox(); // 필요시 사용된 박스로 스프라이트 변경
            }
        }
    }

    void SpawnCoin()
    {
        Instantiate(coinPrefab, transform.position + Vector3.up, Quaternion.identity);
    }

    void ChangeToUsedBox()
    {
        // 여기에 사용된 박스 이미지로 바꾸는 코드 (선택)
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("사용된박스");
    }
}
