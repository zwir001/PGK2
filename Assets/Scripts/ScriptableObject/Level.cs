using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySpawnEvent
{
    public GameObject enemyToSpawn;
    public float timeBetweenSpawn; // time between individual spawned enemy
    public int spawnCount;
    public float timeEndEvent; // how much time we need to wait until next level event
}

[CreateAssetMenu(menuName = "New Level")]
public class Level: ScriptableObject
{
    public int levelNumber;
    public List<EnemySpawnEvent> enemiesSpawnEvents;
}
