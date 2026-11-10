using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemySpawn enemySpawner;

    [FormerlySerializedAs("waves")]
    [SerializeField] private Wave[] easyWaves =
    {
        new Wave(3, 0, 3f),
        new Wave(5, 0, 2.5f)
    };

    [SerializeField] private Wave[] hardWaves =
    {
        new Wave(5, 2, 2f),
        new Wave(8, 5, 1.5f),
        new Wave(10, 10, 1f)
    };

    [SerializeField] private bool overrideDifficulty;
    [SerializeField] private Difficulty testDifficulty = Difficulty.Easy;

    private readonly List<Enemy> aliveEnemies = new List<Enemy>();
    private Wave[] waves;
    private int currentWave = 0;

    public Difficulty CurrentDifficulty { get; private set; }

    private void Start()
    {
        CurrentDifficulty = overrideDifficulty ? testDifficulty : GameSettings.Difficulty;
        waves = GetWaves(CurrentDifficulty);
        Debug.Log("Dificuldade: " + CurrentDifficulty + " | inimigos: " + TotalEnemies());
        StartCoroutine(PlayWaves());
    }

    private Wave[] GetWaves(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Hard:
                return hardWaves;
            default:
                return easyWaves;
        }
    }

    public int TotalEnemies()
    {
        int total = 0;
        foreach (Wave wave in waves)
            total += wave.enemyCount + wave.enemy2Count;
        return total;
    }

    private IEnumerator PlayWaves()
    {
        while (currentWave < waves.Length)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWave]));

            while (true)
            {
                aliveEnemies.RemoveAll(enemy => enemy == null);

                if (aliveEnemies.Count == 0)
                    break;

                yield return new WaitForSeconds(0.5f);
            }
            currentWave++;
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        int spawnedEnemy1 = 0;
        int spawnedEnemy2 = 0;
        bool spawnFirst = true;

        while (spawnedEnemy1 < wave.enemyCount || spawnedEnemy2 < wave.enemy2Count)
        {
            Enemy enemy = null;

            if (spawnFirst && spawnedEnemy1 < wave.enemyCount)
            {
                enemy = enemySpawner.SpawnEnemy();
                spawnedEnemy1++;
            }
            else if (spawnedEnemy2 < wave.enemy2Count)
            {
                enemy = enemySpawner.spawnEnemy2();
                spawnedEnemy2++;
            }
            else
            {
                enemy = enemySpawner.SpawnEnemy();
                spawnedEnemy1++;
            }

            if (enemy != null)
                aliveEnemies.Add(enemy);

            spawnFirst = !spawnFirst;

            if (spawnedEnemy1 < wave.enemyCount || spawnedEnemy2 < wave.enemy2Count)
                yield return new WaitForSeconds(wave.spawnInterval);
        }
    }
}