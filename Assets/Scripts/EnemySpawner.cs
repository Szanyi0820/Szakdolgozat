using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // Assign your enemy prefab in the Inspector
    private List<Vector3> spawnPositions = new List<Vector3>();
    private List<Quaternion> spawnRotations = new List<Quaternion>();
    private List<GameObject> enemies = new List<GameObject>();
    [System.Serializable]
    public class EnemyData
    {
        public Transform spawnPoint;
        public GameObject enemyPrefab;
        public Transform[] patrolPoints;
    }

    public List<EnemyData> enemiesToSpawn = new List<EnemyData>();
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        SpawnAllEnemies();
    }

    public void SpawnAllEnemies()
    {
        foreach (EnemyData enemyData in enemiesToSpawn)
        {
            GameObject newEnemy = Instantiate(enemyData.enemyPrefab, enemyData.spawnPoint.position, enemyData.spawnPoint.rotation);
            AIController aiController = newEnemy.GetComponent<AIController>();

            if (aiController != null)
            {
                aiController.waypoints = enemyData.patrolPoints; // Restore patrol points
            }

            activeEnemies.Add(newEnemy);
        }
    }

    public void RespawnEnemies()
    {
        // Destroy existing enemies
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }
        activeEnemies.Clear();

        // Respawn new enemies
        SpawnAllEnemies();
    }
}
