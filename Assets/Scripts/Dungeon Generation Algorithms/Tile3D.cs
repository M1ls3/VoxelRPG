using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/3D Object Tile")]
public class Tile3D : TileBase
{
    public GameObject prefab; // 3D ��'���


    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        // ������������ ��� ��������� ������, ���� �������
        base.RefreshTile(position, tilemap);
    }

    //public override void SetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    //{
    //    // ����� ����������� ��� ��� �����������, ��� �� ������ ��������������� 3D ��'����
    //}

    // �����������, ���� ���� ���������
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (prefab != null)
        {
            // ������������ 3D ��'��� �� ������� �����
            GameObject.Instantiate(prefab, tilemap.GetComponent<Tilemap>().CellToWorld(position), Quaternion.identity);
        }
        return true;
    }
}
