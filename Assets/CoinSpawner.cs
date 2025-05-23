using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject CoinPrefab;
    public int CoinCount = 10;

    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -2f;
    public float maxY = 2f;

    void Start()
    {
        for (int i = 0; i < CoinCount; i++)
        {
            Vector2 spawnPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            Instantiate(CoinPrefab, spawnPos, Quaternion.identity);
        }
    }
}
