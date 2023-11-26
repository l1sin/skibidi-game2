using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _character;
    [SerializeField] private List<GameObject> _enemiesToSpawn;
    [SerializeField] private float _enemiesCount;
    [SerializeField] private float _minSpawnRadius;
    [SerializeField] private float _maxSpawnRadius;
    [SerializeField] private float _spawnPerSecond;
    private float _spawnPeriod;
    private float _spawnTimer;

    private void Start()
    {
        _spawnPeriod = 1 / _spawnPerSecond;
        _spawnTimer = _spawnPeriod;
        for (int i = 0; i < _enemiesCount; i++)
        {
            SpawnEnemy(_enemiesToSpawn[Random.Range(0, _enemiesToSpawn.Count)]);
        }
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            _spawnTimer = _spawnPeriod;
            SpawnEnemy(_enemiesToSpawn[Random.Range(0, _enemiesToSpawn.Count)]);
        }
    }

    private void SpawnEnemy(GameObject enemyToSpawn)
    {
        Vector3 randomPosition;
        randomPosition = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        randomPosition *= Random.Range(_minSpawnRadius, _maxSpawnRadius);

        GameObject newEnemy = Instantiate(enemyToSpawn, randomPosition, Quaternion.identity);
        Enemy enmey = newEnemy.GetComponent<Enemy>();
        enmey.FollowTarget = _character;
    }
}
