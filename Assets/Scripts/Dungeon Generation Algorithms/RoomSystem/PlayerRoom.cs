using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoom : RoomGenerator
{
    public GameObject player;

    public List<ItemPlacementData> itemData;

    [SerializeField]
    private PrefabPlacer prefabPlacer;

    public override List<GameObject> ProcessRoom(
        Vector3Int roomCenter,
        HashSet<Vector3Int> roomFloor,
        HashSet<Vector3Int> roomFloorNoCorridors)
    {

        ItemPlacementHelper itemPlacementHelper =
            new ItemPlacementHelper(roomFloor, roomFloorNoCorridors);

        List<GameObject> placedObjects =
            prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);

        Vector3Int playerSpawnPoint = roomCenter;

        GameObject playerObject
            = prefabPlacer.CreateObject(player, playerSpawnPoint + new Vector3(0.5f, 1f, 0.5f));

        placedObjects.Add(playerObject);

        return placedObjects;
    }
}

public abstract class PlacementData
{
    [Min(0)]
    public int minQuantity = 0;
    [Min(0)]
    [Tooltip("Max is inclusive")]
    public int maxQuantity = 0;
    public int Quantity
        => UnityEngine.Random.Range(minQuantity, maxQuantity + 1);
}

[Serializable]
public class ItemPlacementData : PlacementData
{
    public ItemData itemData;
}

[Serializable]
public class EnemyPlacementData : PlacementData
{
    public GameObject enemyPrefab;
    public Vector3Int enemySize = Vector3Int.one;
}


[Serializable]
public class DoorPlacementData : PlacementData
{
    public GameObject doorPrefab;
    public Vector3Int doorSize = Vector3Int.one;
}