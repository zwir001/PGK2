using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] private Path _path;

    // Levels
    [Header("Levels")]
    [SerializeField] private List<Level> _levels;
    private Level _currentLevel;
    private bool _levelIsFinished;
    private int _levelIndex;

    // Enemies on map
    [Header("Enemies")]
    [SerializeField] public Transform enemyHolder;

    private void Awake()
    {
        _levelIndex = 0;
    }

    // Spawn Enemies
    public void SpawnEnemyCoroutine()
    {
        _levelIsFinished = false;
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        // next events
        for (int j = 0; j < _currentLevel.enemiesSpawnEvents.Count; j++)
        {
            var levelEvent = _currentLevel.enemiesSpawnEvents[j]; // current level event

            // spawning enemies in events
            for (int i = 0; i < levelEvent.spawnCount; i++)
            {
                Instantiate(levelEvent.enemyToSpawn, _path.spawn.transform.position, Quaternion.identity, enemyHolder);
                yield return new WaitForSeconds(levelEvent.timeBetweenSpawn);
            }
            yield return new WaitForSeconds(levelEvent.timeEndEvent);
        }
        while (GetNumberOfEnemies() > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void LoadNextLevel()
    {
        if (GetNumberOfEnemies() <= 0)
        {
            _currentLevel = _levels[_levelIndex];
            _levelIndex++;
            SpawnEnemyCoroutine();
        }
    }

    public int GetNumberOfEnemies()
    {
        return enemyHolder.childCount;
    }
}
