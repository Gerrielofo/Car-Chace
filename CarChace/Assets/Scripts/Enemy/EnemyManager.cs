using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] List<Enemy> _enemies = new();
    [SerializeField] Waypoint[] _allWaypoints;


    [Header("Settings")]
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] int _enemiesToSpawn = 3;
    [SerializeField] float _spawnDelay = 1f;
    [SerializeField] Color[] _possibleColors;

    [Header("Info")]
    [SerializeField] int _enemyCount;
    [SerializeField] bool _isSpawning;

    private void Start()
    {
        _allWaypoints = FindObjectsOfType<Waypoint>();
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < allEnemies.Length; i++)
        {
            _enemies.Add(allEnemies[i]);
        }

        _enemyCount = _enemies.Count;

        if (_enemyCount < _enemiesToSpawn)
        {
            StartCoroutine(SpawnEnemy(_enemiesToSpawn - _enemyCount));
        }
    }

    private void Update()
    {
        _enemyCount = _enemies.Count;
        if (_enemyCount < _enemiesToSpawn && !_isSpawning)
        {
            StartCoroutine(SpawnEnemy(_enemiesToSpawn - _enemyCount));
        }
    }

    IEnumerator SpawnEnemy(int amountToSpawn)
    {
        Debug.Log("IsSpawning");
        _isSpawning = true;

        Transform randomTransform = GetRandomSpawnPoint();
        _enemies.Add(Instantiate(_enemyPrefab, randomTransform.position, randomTransform.rotation).GetComponent<Enemy>());
        _enemyCount = _enemies.Count;

        int randomColorIndex = Random.Range(0, _possibleColors.Length);

        _enemies[_enemies.Count - 1].GetComponent<CarAgentFollow>().ChangeColor(_possibleColors[randomColorIndex]);

        yield return new WaitForSeconds(_spawnDelay);

        if (amountToSpawn > 0)
        {
            StartCoroutine(SpawnEnemy(amountToSpawn - 1));
        }
        else
        {
            _isSpawning = false;
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }

    Transform GetRandomSpawnPoint()
    {
        if (_allWaypoints.Length != 0)
        {
            return _allWaypoints[Random.Range(0, _allWaypoints.Length)].transform;
        }

        Debug.LogError("Found No Waypoints!");
        return null;
    }
}