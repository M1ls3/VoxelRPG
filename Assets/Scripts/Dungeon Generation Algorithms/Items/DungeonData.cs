using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonData
{
    public Dictionary<Vector3Int, HashSet<Vector3Int>> roomsDictionary;
    public HashSet<Vector3Int> floorPositions;
    public HashSet<Vector3Int> corridorPositions;

    public HashSet<Vector3Int> GetRoomFloorWithoutCorridors(Vector3Int dictionaryKey)
    {
        HashSet<Vector3Int> roomFloorNoCorridors = new HashSet<Vector3Int>(roomsDictionary[dictionaryKey]);
        roomFloorNoCorridors.ExceptWith(corridorPositions);
        return roomFloorNoCorridors;
    }
}
