
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyPrefab2;
    public Transform[] waypoints;

    public Enemy SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.SetWayPoints(waypoints);
        return enemyScript;
    }

    public Enemy spawnEnemy2()
    {
        GameObject enemy = Instantiate(enemyPrefab2, transform.position, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.SetWayPoints(waypoints);
        return enemyScript;
    }
}
