using Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [Serializable]
    public struct Wave
    {
        public float timeLimit;
        public float spawnTime;
        public WaveEnemyListItem[] enemies;
    }

    [Serializable]
    public struct WaveEnemyListItem
    {
        public GameObject enemyPrefab;
        public int count;
    }

    private static WaveController instance;
    private readonly List<Enemy> enemies = new();
    private SpawnPoint[] spawnPoints;
    [SerializeField] private Wave[] waves;
    private int currentWaveIdx = -1;
    private int enemiesLeft = 0;
    private IEnumerator waveTimer;

    public static WaveController Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        StartNextWave();
    }

    public void RegisterEnemy(Enemy enemy) => enemies.Add(enemy);
    public void UnregisterEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        enemiesLeft--;
        Debug.Log(enemiesLeft);
        if (enemiesLeft == 0) OnWaveEnd();
    }
    
    public void OnWaveEnd()
    {
        if (!enabled) return;
        StartNextWave();
        if (currentWaveIdx < waves.Length)
        {
            var cardPicker = CardPicker.Instance;
            if (cardPicker != null) cardPicker.Show();
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        int enemyCount = 0;
        foreach (var item in wave.enemies)
            enemyCount += item.count;

        enemiesLeft += enemyCount;
        float spawnInterval = wave.spawnTime / enemyCount;

        foreach (var item in wave.enemies)
            for (int i = 0; i < item.count; i++)
            {
                var spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length - 1)];
                if (spawnPoint == null) {
                    Debug.LogWarning("Spawn point doesn't exist, skipping");
                    break;
                }
                spawnPoint.Spawn(item.enemyPrefab);
                yield return new WaitForSeconds(spawnInterval);
            }
    }

    private IEnumerator WaveTimer(Wave wave)
    {
        yield return new WaitForSeconds(wave.timeLimit);
        OnWaveEnd();
    }

    public void StartNextWave()
    {
        if (waveTimer != null) StopCoroutine(waveTimer);

        if (currentWaveIdx < waves.Length - 1)
            currentWaveIdx++;

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points found");
            return;
        }

        Wave wave = waves[currentWaveIdx];
        StartCoroutine(SpawnWave(wave));

        if (wave.timeLimit > 0)
        {
            waveTimer = WaveTimer(wave);
            StartCoroutine(waveTimer);
        }
    }
}
