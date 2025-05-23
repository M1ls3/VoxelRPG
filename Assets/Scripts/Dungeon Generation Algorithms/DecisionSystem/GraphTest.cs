using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphTest : MonoBehaviour
{
    Graph graph;

    bool graphReady = false;

    Dictionary<Vector3Int, int> dijkstraResult;
    int highestValue;

    public Graph Graph { get { return graph; } }
    public bool GraphReady { get { return graphReady; } }
    public Dictionary<Vector3Int, int> DijkstraResult { get { return dijkstraResult; } }
    public int HighestValue { get { return highestValue; } }

    public void RunDijkstraAlgorithm(Vector3Int playerPosition, IEnumerable<Vector3Int> floorPositions)
    {
        graphReady = false;
        graph = new Graph(floorPositions);
        dijkstraResult = DijkstraAlgorithm.Dijkstra(graph, playerPosition);
        highestValue = dijkstraResult.Values.Max();
        graphReady = true;
    }


    private void OnDrawGizmosSelected()
    {
        if (graphReady && dijkstraResult != null)
        {
            foreach (var item in dijkstraResult)
            {
                Color color = Color.Lerp(Color.green, Color.red, (float)item.Value / highestValue);
                color.a = 0.5f;
                Gizmos.color = color;
                Gizmos.DrawCube(item.Key + new Vector3(0.5f, 0.5f), Vector3.one);
            }
        }
    }
}
