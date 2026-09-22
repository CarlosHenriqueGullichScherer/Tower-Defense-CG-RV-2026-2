using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] waypoints;
    public float spawnInterval = 3f;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity
            );

            Enemy enemyScript = enemy.GetComponent<Enemy>();

            enemyScript.SetWayPoints(waypoints);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}