using Characters;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    private static WaveController instance;
    private readonly List<Enemy> enemies = new();

    public static WaveController Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public void RegisterEnemy(Enemy enemy) => enemies.Add(enemy);
    public void UnregisterEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0) OnWaveEnd();
    }
    
    public void OnWaveEnd()
    {
        CardPicker.Instance.Show();
        StartNextWave();
    }

    public void StartNextWave()
    {

    }
}
