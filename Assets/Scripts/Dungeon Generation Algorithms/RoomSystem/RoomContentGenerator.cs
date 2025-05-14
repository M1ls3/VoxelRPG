using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class RoomContentGenerator : MonoBehaviour
{
    [SerializeField]
    private RoomGenerator playerRoom, defaultRoom, portalRoom;

    List<GameObject> spawnedObjects = new List<GameObject>();

    [SerializeField]
    private GraphTest graphTest;

    public Transform itemParent;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineCamera;

    public UnityEvent RegenerateDungeon;

    public void GenerateRoomContent(DungeonData dungeonData)
    {

        foreach (GameObject item in spawnedObjects)
        {
            DestroyImmediate(item);
        }
        spawnedObjects.Clear();

        SelectPlayerSpawnPoint(dungeonData);
        SelectDoorSpawnPoints(dungeonData);
        SelectEnemySpawnPoints(dungeonData);

        foreach (GameObject item in spawnedObjects)
        {
            if (item != null)
                item.transform.SetParent(itemParent, false);
        }
    }

    private void SelectPlayerSpawnPoint(DungeonData dungeonData)
    {

        if (dungeonData.roomsDictionary == null || dungeonData.roomsDictionary.Count == 0)
        {
            Debug.LogError("RoomsDictionary is empty or null!");
            return;
        }

        int randomRoomIndex = UnityEngine.Random.Range(0, dungeonData.roomsDictionary.Count);
        Vector3Int playerSpawnPoint = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        if (!dungeonData.roomsDictionary.ContainsKey(playerSpawnPoint))
        {
            Debug.LogError("Invalid spawn point selected!");
            return;
        }

        graphTest.RunDijkstraAlgorithm(playerSpawnPoint, dungeonData.floorPositions);

        Vector3Int roomIndex = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        List<GameObject> placedPrefabs = playerRoom.ProcessRoom(
            playerSpawnPoint,
            dungeonData.roomsDictionary.Values.ElementAt(randomRoomIndex),
            dungeonData.GetRoomFloorWithoutCorridors(roomIndex)
            );

        FocusCameraOnThePlayer(placedPrefabs[placedPrefabs.Count - 1].transform);

        spawnedObjects.AddRange(placedPrefabs);

        dungeonData.roomsDictionary.Remove(playerSpawnPoint);
    }

    private void SelectDoorSpawnPoints(DungeonData dungeonData)
    {
        // 1. Проверяем наличие данных алгоритма Дейкстры
        if (graphTest.DijkstraResult == null || graphTest.DijkstraResult.Count == 0)
        {
            Debug.LogError("Dijkstra data not available!");
            return;
        }

        // 2. Находим комнату с максимальным расстоянием от игрока
        var farthestRoom = dungeonData.roomsDictionary.Keys
            .OrderByDescending(roomPos => graphTest.DijkstraResult.ContainsKey(roomPos) ?
                graphTest.DijkstraResult[roomPos] : -1)
            .FirstOrDefault();

        // 3. Проверяем валидность найденной комнаты
        if (!dungeonData.roomsDictionary.ContainsKey(farthestRoom))
        {
            Debug.LogError("Failed to find farthest room!");
            return;
        }

        // 4. Генерируем содержимое комнаты с дверью
        List<GameObject> doorRoomObjects = portalRoom.ProcessRoom(
            farthestRoom,
            dungeonData.roomsDictionary[farthestRoom],
            dungeonData.GetRoomFloorWithoutCorridors(farthestRoom)
        );

        // 6. Регистрируем созданные объекты
        spawnedObjects.AddRange(doorRoomObjects);

        // 7. Удаляем комнату из словаря для предотвращения повторной обработки
        dungeonData.roomsDictionary.Remove(farthestRoom);
    }

    private void SelectEnemySpawnPoints(DungeonData dungeonData)
    {
        foreach (KeyValuePair<Vector3Int, HashSet<Vector3Int>> roomData in dungeonData.roomsDictionary)
        {
            spawnedObjects.AddRange(
                defaultRoom.ProcessRoom(
                    roomData.Key,
                    roomData.Value,
                    dungeonData.GetRoomFloorWithoutCorridors(roomData.Key)
                    )
            );

        }
    }

    private void FocusCameraOnThePlayer(Transform playerTransform)
    {
        cinemachineCamera.LookAt = playerTransform;
        cinemachineCamera.Follow = playerTransform;
    }

    public void ClearAllChildren()
    {
        if (itemParent == null)
        {
            Debug.LogError("no ItemParent");
            return;
        }

        try
        {
            foreach (Transform child in itemParent)
            {
                Destroy(child.gameObject);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Clean error: {e.Message}");
        }
    }
}
