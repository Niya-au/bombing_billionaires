using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [Header("Hazard")]
    [SerializeField] private GameObject hazardPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnWidth = 8f;
    [SerializeField] private float spawnHeight = 6f;

    [Header("Level 3")]
    [SerializeField] private MoneySpawner moneySpawner;

    private float spawnTimer = 0f;
    private bool spawningEnabled = true;

    private void Update()
    {
        // Stop spawning when HazardPhase tells us to stop
        if (!spawningEnabled)
            return;

        // Wait until $1000 has been collected
        if (moneySpawner == null)
            return;

        if (!moneySpawner.GoalReached)
            return;

        // Make sure a hazard prefab exists
        if (hazardPrefab == null)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnHazard();
            spawnTimer = 0f;
        }
    }

    private void SpawnHazard()
    {
        float randomX = Random.Range(-spawnWidth, spawnWidth);

        Vector3 spawnPosition = new Vector3(
            transform.position.x + randomX,
            spawnHeight,
            0f
        );

        Instantiate(
            hazardPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }

    public void StopSpawning()
    {
        spawningEnabled = false;

        Debug.Log("Hazard spawning stopped.");
    }
}