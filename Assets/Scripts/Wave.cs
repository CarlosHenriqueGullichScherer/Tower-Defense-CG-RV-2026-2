using UnityEngine;

[System.Serializable]
public class Wave
{
    public int enemyCount = 5;
    public int enemy2Count = 3;
    public float spawnInterval = 5f;

    public Wave() { }

    public Wave(int enemyCount, int enemy2Count, float spawnInterval)
    {
        this.enemyCount = enemyCount;
        this.enemy2Count = enemy2Count;
        this.spawnInterval = spawnInterval;
    }
}