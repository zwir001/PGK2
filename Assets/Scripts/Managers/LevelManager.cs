using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] private Path _path;
    private float _pathLength;

    // Levels
    [Header("Levels")]
    [SerializeField] private List<Level> _levels;
    private Level _currentLevel;
    //private bool _levelIsFinished;
    private int _levelIndex;

    [Header("UI")]
    [SerializeField] private Button _nextRoundButton;

    // Enemies on map
    [Header("Enemies")]
    [SerializeField] public Transform enemyHolder;

    private float timer;
    private float timeToDisplay;
    private bool _gameStart;

    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _levelIndex = 0;
        _pathLength = 0;
        CalculatePathLength();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        timeToDisplay = Mathf.FloorToInt(timer % 60);
    }

    // Spawn Enemies
    public void SpawnEnemyCoroutine()
    {
        //_levelIsFinished = false;
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
        _nextRoundButton.gameObject.SetActive(true);
    }

    public void LoadNextLevel()
    {
        if (GetNumberOfEnemies() <= 0)
        {
            _currentLevel = _levels[_levelIndex];

            if (_levelIndex == 0)
                _gameStart = true;

            _levelIndex++;
            _nextRoundButton.gameObject.SetActive(false);
            SpawnEnemyCoroutine();
        }
    }

    private void CalculatePathLength()
    {
        var firstCycle = true;
        for (int i = 0; i < (_path.points.Count - 1); i++)
        {
            if (firstCycle)
            {
                _pathLength += Vector3.Distance(_path.spawn.transform.position, _path.points[i].transform.position);
                firstCycle = false;
                i--;
            }
            else
            {
                _pathLength += Vector3.Distance(_path.points[i].transform.position, _path.points[i + 1].transform.position);
            }
        }
    }

    public int GetLevelIndex()
    {
        return _levelIndex;
    }

    public int GetLevelsCount()
    {
        return _levels.Count;
    }

    public int GetNumberOfEnemies()
    {
        return enemyHolder.childCount;
    }

    public float GetTime()
    {
        return timeToDisplay;
    }

    public bool IsGameStarted()
    {
        return _gameStart;
    }

    public float GetPathLength()
    {
        return _pathLength;
    }
}
