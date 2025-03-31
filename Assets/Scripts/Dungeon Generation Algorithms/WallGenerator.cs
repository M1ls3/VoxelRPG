using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector3Int> floorPosition, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPosition, Direction3D.cardinalDirectionList);
        foreach (var pos in basicWallPositions)
        {
            tilemapVisualizer.PaintSingleBasicWall(pos);
        }
    }

    private static HashSet<Vector3Int> FindWallsInDirections(HashSet<Vector3Int> floorPosition, List<Vector3Int> directionList)
    {
        HashSet<Vector3Int> wallPositions = new HashSet<Vector3Int>();
        foreach (var pos in floorPosition)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = pos + direction;
                if (floorPosition.Contains(neighbourPosition) == false)
                    wallPositions.Add(neighbourPosition);
            }
        }
        return wallPositions;
    }
}
