using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PropData
{
    public GameObject prefab;
    public float spawnProbability = 0.1f;
    public bool spawnAlongWalls = true;
    public float minDistanceFromOtherProps = 1f;
}

[System.Serializable]
public class EnemyData
{
    public GameObject prefab;
    public int minEnemiesPerRoom = 1;
    public int maxEnemiesPerRoom = 3;
}
