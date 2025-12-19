using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Enemy Prefabs (size must be 2)")]
    [SerializeField] private GameObject[] enemyPrefabs = new GameObject[2];

    [Header("Spawn Settings")]
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private bool autoSpawn = true;

    void Start()
    {
        if (autoSpawn)
            InvokeRepeating(nameof(SpawnRandomEnemy), startDelay, spawnInterval);
    }

    public void SpawnRandomEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length < 2)
        {
            Debug.LogError("EnemySpawner: Assign 2 enemy prefabs in the Inspector.");
            return;
        }

        int index = Random.Range(0, 2); // 0 or 1
        Instantiate(enemyPrefabs[index], transform.position, Quaternion.identity);
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(SpawnRandomEnemy));
    }

    public void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnRandomEnemy), startDelay, spawnInterval);
    }
}
