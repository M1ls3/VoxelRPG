using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/3D Object Tile")]
public class Tile3D : TileBase
{
    public GameObject prefab; // 3D об'єкт


    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        // Налаштування для оновлення плиток, якщо потрібно
        base.RefreshTile(position, tilemap);
    }

    //public override void SetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    //{
    //    // Можна налаштувати дані для відображення, але ми будемо використовувати 3D об'єкти
    //}

    // Викликається, коли тайл малюється
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (prefab != null)
        {
            // Інстанціюємо 3D об'єкт на позиції тайла
            GameObject.Instantiate(prefab, tilemap.GetComponent<Tilemap>().CellToWorld(position), Quaternion.identity);
        }
        return true;
    }
}
