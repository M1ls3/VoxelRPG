using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CorridorFirstDungeonGeneration : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;

    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;
    
    //PCG Data
    private Dictionary<Vector3Int, HashSet<Vector3Int>> roomDictionary
        = new Dictionary<Vector3Int, HashSet<Vector3Int>>();

    private HashSet<Vector3Int> floorPositions, corridorPositions;

    //Gizmos Data
    private List<Color> roomColors = new List<Color>();
    [SerializeField]
    private bool showRoomGizmo = false, showCorridorsGizmo;

    //Events
    public UnityEvent<DungeonData> OnDungeonFloorReady;

    [SerializeField]
    [Range(0, 3)]
    private int corridorWidth = 3;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
        DungeonData data = new DungeonData
        {
            roomsDictionary = this.roomDictionary,
            corridorPositions = this.corridorPositions,
            floorPositions = this.floorPositions
        };
        OnDungeonFloorReady?.Invoke(data);
    }

    private void CorridorFirstGeneration()
    {
        floorPositions = new HashSet<Vector3Int>();
        HashSet<Vector3Int> potentialRoomPositions = new HashSet<Vector3Int>();

        List<List<Vector3Int>>  corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector3Int> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector3Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        ExpandCorridors(corridors);

        if (roomDictionary.Count > 4)
        {
            tilemapVisualizer.PaintFloorTiles(floorPositions);
            WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
        }
        else RunProceduralGeneration();

    }

    private void ExpandCorridors(List<List<Vector3Int>> corridors)
    {
        for (int i = 0; i < corridors.Count; i++)
        {
            //corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
            corridors[i] = IncreaseCorridorBrush3by3(corridors[i], Math.Abs(corridorWidth));
            floorPositions.UnionWith(corridors[i]);
        }
    }

    private List<Vector3Int> IncreaseCorridorBrush3by3(List<Vector3Int> corridor, int corridorWidth)
    {
        List<Vector3Int> newCorridor = new List<Vector3Int>();

        for (int i = 1; i < corridor.Count; i++)
        {
            for (int x = -1; x < corridorWidth - 1; x++)
            {
                for (int y = -1; y < corridorWidth - 1; y++)
                {
                    newCorridor.Add(corridor[i - 1] + new Vector3Int(x, 0, y));
                }
            }
        }
        return newCorridor;
    }

    private List<List<Vector3Int>> CreateCorridors(HashSet<Vector3Int> floorPositions, HashSet<Vector3Int> potentialRoomPositions)
    {
        var currentPosition = startPos;
        potentialRoomPositions.Add(currentPosition);
        List<List<Vector3Int>> corridors = new List<List<Vector3Int>>();

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithm.RandomWalkCorridor(currentPosition, corridorLength);
            corridors.Add(corridor);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
        //ExpandCorridors(corridors);
        corridorPositions = new HashSet<Vector3Int>(floorPositions);
        return corridors;
    }

    private Vector3Int GetDirection90From(Vector3Int direction)
    {
        if (direction == Vector3Int.forward)
            return Vector3Int.right;
        if (direction == Vector3Int.right)
            return Vector3Int.back;
        if (direction == Vector3Int.back)
            return Vector3Int.left;
        if (direction == Vector3Int.left)
            return Vector3Int.forward;
        return Vector3Int.zero;
    }

    private IEnumerator GenerateRoomsCoroutine(HashSet<Vector3Int> potentialRoomPositions)
    {
        yield return new WaitForSeconds(2);
        tilemapVisualizer.Clear();
        //GenerateRooms(potentialRoomPositions);
        DungeonData data = new DungeonData
        {
            roomsDictionary = this.roomDictionary,
            corridorPositions = this.corridorPositions,
            floorPositions = this.floorPositions
        };
        OnDungeonFloorReady?.Invoke(data);
    }

    private void CreateRoomsAtDeadEnd(List<Vector3Int> deadEnds, HashSet<Vector3Int> roomFloors)
    {
        foreach (var pos in deadEnds)
        {
            if (roomFloors.Contains(pos) == false)
            {
                var roomFloor = RunRumdomWalk(randomWalkParameters, pos);
                SaveRoomData(pos, roomFloor);
                roomFloors.UnionWith(roomFloor);
            }
        }
    }

    private List<Vector3Int> FindAllDeadEnds(HashSet<Vector3Int> floorPositions)
    {
        List<Vector3Int> deadEnds = new List<Vector3Int>();

        foreach (var pos in floorPositions)
        {
            int neighboursCount = 0;

            foreach (var direction in Direction3D.cardinalDirectionList)
            {
                if (floorPositions.Contains(pos + direction))
                    neighboursCount++;
            }

            if (neighboursCount == 1)
                deadEnds.Add(pos);
        }
        return deadEnds;
    }

    private HashSet<Vector3Int> CreateRooms(HashSet<Vector3Int> potentialRoomPositions)
    {
        HashSet<Vector3Int> roomPositions = new HashSet<Vector3Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector3Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        ClearRoomData();
        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRumdomWalk(randomWalkParameters, roomPosition);

            SaveRoomData(roomPosition, roomFloor);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private void ClearRoomData()
    {
        roomDictionary.Clear();
        roomColors.Clear();
    }

    private void SaveRoomData(Vector3Int roomPosition, HashSet<Vector3Int> roomFloor)
    {
        roomDictionary[roomPosition] = roomFloor;
        roomColors.Add(UnityEngine.Random.ColorHSV());
    }

    private void OnDrawGizmosSelected()
    {
        if (showRoomGizmo)
        {
            int i = 0;
            foreach (var roomData in roomDictionary)
            {
                Color color = roomColors[i];
                color.a = 0.5f;
                Gizmos.color = color;
                Gizmos.DrawSphere((Vector3)roomData.Key, 0.5f);
                foreach (var position in roomData.Value)
                {
                    Gizmos.DrawCube((Vector3)position + new Vector3(0.5f, 0.5f), Vector3.one);
                }
                i++;
            }
        }
        if (showCorridorsGizmo && corridorPositions != null)
        {
            Gizmos.color = Color.magenta;
            foreach (var corridorTile in corridorPositions)
            {
                Gizmos.DrawCube((Vector3)corridorTile + new Vector3(0.5f, 0.5f), Vector3.one);
            }
        }
    }
}
