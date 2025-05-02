using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomGenerator : MonoBehaviour
{
    public abstract List<GameObject> ProcessRoom(
        Vector3Int roomCenter,
        HashSet<Vector3Int> roomFloor,
        HashSet<Vector3Int> corridors);
}
