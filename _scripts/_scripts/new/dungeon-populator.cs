using UnityEngine;
using System.Collections.Generic;

public class DungeonPopulator : MonoBehaviour
{
    [SerializeField] private PropData[] propsList;
    [SerializeField] private EnemyData[] enemiesList;
    [SerializeField] private GameObject exitPrefab;
    [SerializeField] private GameObject playerPrefab;
    
    private HashSet<Vector2Int> spawnedObjectsPositions = new HashSet<Vector2Int>();
    private Vector2Int playerSpawnRoom;
    private List<BoundsInt> roomsList;
    
    public void PopulateDungeon(List<BoundsInt> rooms, HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> wallPositions)
    {
        roomsList = rooms;
        ClearExistingObjects();
        
        // Select random room for player spawn
        BoundsInt playerRoom = rooms[Random.Range(0, rooms.Count)];
        playerSpawnRoom = new Vector2Int(
            Mathf.RoundToInt(playerRoom.center.x),
            Mathf.RoundToInt(playerRoom.center.y));
        
        // Spawn player
        SpawnPlayer(playerSpawnRoom);
        
        // Select random room (not player room) for exit
        BoundsInt exitRoom;
        do {
            exitRoom = rooms[Random.Range(0, rooms.Count)];
        } while (exitRoom == playerRoom);
        
        Vector2Int exitPosition = new Vector2Int(
            Mathf.RoundToInt(exitRoom.center.x),
            Mathf.RoundToInt(exitRoom.center.y));
        SpawnExit(exitPosition);
        
        // Spawn enemies in rooms (except player room)
        foreach (var room in rooms)
        {
            Vector2Int roomCenter = new Vector2Int(
                Mathf.RoundToInt(room.center.x),
                Mathf.RoundToInt(room.center.y));
                
            if (roomCenter != playerSpawnRoom)
            {
                SpawnEnemiesInRoom(room, floorPositions);
            }
        }
        
        // Spawn props
        SpawnProps(floorPositions, wallPositions);
    }
    
    private void ClearExistingObjects()
    {
        // Clear all existing enemies, props, and exits
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Enemy") || child.CompareTag("Prop") || child.CompareTag("Exit"))
            {
                Destroy(child.gameObject);
            }
        }
        spawnedObjectsPositions.Clear();
    }
    
    private void SpawnPlayer(Vector2Int position)
    {
        Instantiate(playerPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        spawnedObjectsPositions.Add(position);
    }
    
    private void SpawnExit(Vector2Int position)
    {
        Instantiate(exitPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        spawnedObjectsPositions.Add(position);
    }
    
    private void SpawnEnemiesInRoom(BoundsInt room, HashSet<Vector2Int> floorPositions)
    {
        if (enemiesList.Length == 0) return;
        
        EnemyData enemyData = enemiesList[Random.Range(0, enemiesList.Length)];
        int enemiesToSpawn = Random.Range(enemyData.minEnemiesPerRoom, enemyData.maxEnemiesPerRoom + 1);
        
        List<Vector2Int> possiblePositions = new List<Vector2Int>();
        
        // Get all floor positions in this room
        for (int x = room.xMin; x < room.xMax; x++)
        {
            for (int y = room.yMin; y < room.yMax; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (floorPositions.Contains(pos) && !spawnedObjectsPositions.Contains(pos))
                {
                    possiblePositions.Add(pos);
                }
            }
        }
        
        // Spawn enemies
        for (int i = 0; i < enemiesToSpawn && possiblePositions.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, possiblePositions.Count);
            Vector2Int spawnPos = possiblePositions[randomIndex];
            
            GameObject enemy = Instantiate(enemyData.prefab, new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);
            enemy.transform.parent = transform;
            enemy.tag = "Enemy";
            
            spawnedObjectsPositions.Add(spawnPos);
            possiblePositions.RemoveAt(randomIndex);
        }
    }
    
    private void SpawnProps(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> wallPositions)
    {
        if (propsList.Length == 0) return;
        
        // Find floor tiles adjacent to walls
        HashSet<Vector2Int> floorTilesNearWalls = new HashSet<Vector2Int>();
        foreach (var wallPos in wallPositions)
        {
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                Vector2Int adjacentPos = wallPos + direction;
                if (floorPositions.Contains(adjacentPos) && !spawnedObjectsPositions.Contains(adjacentPos))
                {
                    floorTilesNearWalls.Add(adjacentPos);
                }
            }
        }
        
        // Create list of positions for props that don't need to be near walls
        List<Vector2Int> otherFloorPositions = new List<Vector2Int>();
        foreach (var pos in floorPositions)
        {
            if (!floorTilesNearWalls.Contains(pos) && !spawnedObjectsPositions.Contains(pos))
            {
                otherFloorPositions.Add(pos);
            }
        }
        
        // Spawn wall props
        foreach (var propData in propsList)
        {
            List<Vector2Int> possiblePositions = propData.spawnAlongWalls ? 
                new List<Vector2Int>(floorTilesNearWalls) : otherFloorPositions;
                
            if (possiblePositions.Count == 0) continue;
            
            for (int i = 0; i < possiblePositions.Count; i++)
            {
                if (Random.value <= propData.spawnProbability)
                {
                    Vector2Int spawnPos = possiblePositions[i];
                    
                    // Check distance from other props
                    bool tooClose = false;
                    foreach (var existingPos in spawnedObjectsPositions)
                    {
                        if (Vector2.Distance(spawnPos, existingPos) < propData.minDistanceFromOtherProps)
                        {
                            tooClose = true;
                            break;
                        }
                    }
                    
                    if (!tooClose)
                    {
                        GameObject prop = Instantiate(propData.prefab, new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);
                        prop.transform.parent = transform;
                        prop.tag = "Prop";
                        
                        spawnedObjectsPositions.Add(spawnPos);
                        
                        // Remove nearby positions from consideration
                        possiblePositions.RemoveAll(pos => 
                            Vector2.Distance(pos, spawnPos) < propData.minDistanceFromOtherProps);
                    }
                }
            }
        }
    }
}
