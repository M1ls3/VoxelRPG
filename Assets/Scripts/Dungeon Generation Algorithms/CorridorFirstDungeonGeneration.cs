using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CorridorFirstDungeonGeneration : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;

    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;

    [SerializeField]
    [Range(0, 3)]
    private int corridorWidth = 3;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();
        HashSet<Vector3Int> potintailRoomPositions = new HashSet<Vector3Int>();

        List<List<Vector3Int>> corridors = CreateCorridors(floorPositions, potintailRoomPositions);

        HashSet<Vector3Int> roomPositions = CreateRooms(potintailRoomPositions);

        List<Vector3Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        ExpandCorridors(corridors, floorPositions);

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private void ExpandCorridors(List<List<Vector3Int>> corridors, HashSet<Vector3Int> floorPositions)
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

    private List<Vector3Int> IncreaseCorridorSizeByOne(List<Vector3Int> corridor)
    {
        List<Vector3Int> newCorridor = new List<Vector3Int>();
        Vector3Int previousDirection = Vector3Int.zero;

        for (int i = 1; i < corridor.Count; i++)
        {
            Vector3Int directionFromCell = corridor[i] - corridor[i - i];
            if (previousDirection != Vector3Int.zero &&
                directionFromCell != previousDirection)
            {
                //handle corner
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector3Int(x, 0, y));
                    }
                }
                previousDirection = directionFromCell;
            }
            else
            {
                //Add a single cell in the direction + 90 degrees
                Vector3Int newCorridorTileOffset
                    = GetDirection90From(directionFromCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
            }
        }
        return newCorridor;
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

    private void CreateRoomsAtDeadEnd(List<Vector3Int> deadEnds, HashSet<Vector3Int> roomFloors)
    {
        foreach (var pos in deadEnds)
        {
            if (roomFloors.Contains(pos) == false)
            {
                var roomFloor = RunRumdomWalk(randomWalkParameters, pos);
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

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRumdomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private List<List<Vector3Int>> CreateCorridors(HashSet<Vector3Int> floorPositions, HashSet<Vector3Int> potintailRoomPositions)
    {
        var currentPosition = startPos;
        potintailRoomPositions.Add(currentPosition);
        List<List<Vector3Int>> corridors = new List<List<Vector3Int>>();

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithm.RandomWalkCorridor(currentPosition, corridorLength);
            corridors.Add(corridor);
            currentPosition = corridor[corridor.Count - 1];
            potintailRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
        return corridors;
    }
}
