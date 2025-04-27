using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DijkstraAlgorithm
{
    public static Dictionary<Vector3Int, int> Dijkstra(Graph graph, Vector3Int startposition)
    {
        Queue<Vector3Int> unfinishedVertices = new Queue<Vector3Int>();

        Dictionary<Vector3Int, int> distanceDictionary = new Dictionary<Vector3Int, int>();
        Dictionary<Vector3Int, Vector3Int> parentDictionary = new Dictionary<Vector3Int, Vector3Int>();

        distanceDictionary[startposition] = 0;
        parentDictionary[startposition] = startposition;

        foreach (Vector3Int vertex in graph.GetNeighbours4Directions(startposition))
        {
            unfinishedVertices.Enqueue(vertex);
            parentDictionary[vertex] = startposition;
        }

        while (unfinishedVertices.Count > 0)
        {
            Vector3Int vertex = unfinishedVertices.Dequeue();
            int newDistance = distanceDictionary[parentDictionary[vertex]] + 1;
            if (distanceDictionary.ContainsKey(vertex) && distanceDictionary[vertex] <= newDistance)
                continue;
            distanceDictionary[vertex] = newDistance;

            foreach (Vector3Int neighbour in graph.GetNeighbours4Directions(vertex))
            {
                if (distanceDictionary.ContainsKey(neighbour))
                    continue;
                unfinishedVertices.Enqueue(neighbour);
                parentDictionary[neighbour] = vertex;
            }
        }

        return distanceDictionary;
    }
}
