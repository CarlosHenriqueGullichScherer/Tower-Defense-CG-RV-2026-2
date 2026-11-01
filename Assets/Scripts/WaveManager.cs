
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemySpawn enemySpawner;
    [SerializeField] private Wave[] waves;
  

    private readonly List<Enemy> aliveEnemies = new List<Enemy>();
    private int currentWave = 0;

    private void Start()
    {
        StartCoroutine(PlayWaves());
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
