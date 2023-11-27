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

    [SerializeField] private Color[] _tierColors;

    [SerializeField] private int _level;

    private float _spawnPeriod;
    private float _spawnTimer;

    private void Start()
    {
        _spawnPeriod = 1 / _spawnPerSecond;
        _spawnTimer = _spawnPeriod;
        for (int i = 0; i < _enemiesCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            _spawnTimer = _spawnPeriod;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int i;
        do
        {
            i = Random.Range(0, _enemiesToSpawn.Count);
        }
        while (i > _level);

        int maxTier = Mathf.Min((_level - i) / 5, _tierColors.Length - 1);
        int tier = Random.Range(0, maxTier + 1);
        GameObject enemyToSpawn = _enemiesToSpawn[i];


        Vector3 randomPosition;
        randomPosition = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        randomPosition *= Random.Range(_minSpawnRadius, _maxSpawnRadius);

        GameObject newEnemy = Instantiate(enemyToSpawn, randomPosition, Quaternion.identity);
        Enemy enemy = newEnemy.GetComponent<Enemy>();

        enemy.SetTier(tier, _tierColors[tier]);
        enemy.FollowTarget = _character;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _minSpawnRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _maxSpawnRadius);
    }
}
