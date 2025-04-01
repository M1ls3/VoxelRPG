using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithm
{
    public static HashSet<Vector3Int> SimpleRandomWalk(Vector3Int startPosition, int walkLength)
    {
        HashSet<Vector3Int> path = new HashSet<Vector3Int>();

        path.Add(startPosition);
        var previousPosition = startPosition;

        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousPosition + Direction3D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }
        return path;
    }

    public static List<Vector3Int> RandomWalkCorridor(Vector3Int startPosition, int corridorLength)
    {
        List<Vector3Int> corridor = new List<Vector3Int>();
        var direction = Direction3D.GetRandomCardinalDirection();
        var currentPosition = startPosition;
        corridor.Add(currentPosition);
        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }
        return corridor;
    }
}


public static class Direction3D
{
    public static List<Vector3Int> cardinalDirectionList = new List<Vector3Int>()
    {
        new Vector3Int(1, 0, 0), //RIGHT
        //new Vector3Int(0, 1, 0), //UP
        new Vector3Int(0, 0, 1), //FORWARD
        new Vector3Int(-1, 0, 0), //LEFT
        //new Vector3Int(0, -1, 0), //DOWN
        new Vector3Int(0, 0, -1), //BACK
    };

    public static Vector3Int GetRandomCardinalDirection()
    {
        return cardinalDirectionList[Random.Range(0,cardinalDirectionList.Count)];
    }
}