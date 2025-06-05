using UnityEngine;

public class CoinBox : MonoBehaviour
{
    public GameObject coinPrefab; // ���� ������
    private bool isUsed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ������ �Ӹ��� �浹�� �����Ϸ��� �浹 ������ �Ʒ����� �ö�� ��츸 üũ
        if (!isUsed && collision.contacts[0].normal.y > 0.5f)
        {
            if (collision.collider.CompareTag("Player")) // �Ǵ� ������ �±�
            {
                SpawnCoin();
                isUsed = true;
                ChangeToUsedBox(); // �ʿ�� ���� �ڽ��� ��������Ʈ ����
            }
        }
    }

    void SpawnCoin()
    {
        Instantiate(coinPrefab, transform.position + Vector3.up, Quaternion.identity);
    }

    void ChangeToUsedBox()
    {
        // ���⿡ ���� �ڽ� �̹����� �ٲٴ� �ڵ� (����)
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("���ȹڽ�");
    }
}
