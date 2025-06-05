using UnityEngine;
using UnityEngine.Tilemaps; // 이것도 필요함!

[CreateAssetMenu(menuName = "Custom Tiles/CoinBoxTile")]
public class CoinBoxTile : Tile
{
    public GameObject coinPrefab;
    public Tile usedTile;

    public void OnHit(Vector3Int position, Tilemap tilemap)
    {
        if (usedTile != null)
        {
            tilemap.SetTile(position, usedTile);
        }

        if (coinPrefab != null)
        {
            Vector3 worldPos = tilemap.CellToWorld(position) + new Vector3(0.5f, 1.5f, 0f);
            GameObject.Instantiate(coinPrefab, worldPos, Quaternion.identity);
        }
    }
}
