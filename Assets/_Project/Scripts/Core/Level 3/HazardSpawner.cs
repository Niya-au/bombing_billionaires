
using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [Header("Hazard")]
    [SerializeField] private GameObject hazardPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnWidth = 8f;
    [SerializeField] private float spawnHeight = 6f;

    private float spawnTimer = 0f;

    private bool phaseFinished = false;

    private void Update()
    {
        // Stop hvis hazard-fasen er færdig
        if (phaseFinished)
            return;

        // Find MoneySpawner i scenen
        MoneySpawner moneySpawner = FindFirstObjectByType<MoneySpawner>();

        if (moneySpawner == null)
            return;

        // Vent på at spilleren har samlet $1000
        if (!moneySpawner.GoalReached)
            return;

        // Spawn hazards
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnHazard();
            spawnTimer = 0f;
        }
    }

    private void SpawnHazard()
    {
        if (hazardPrefab == null)
        {
            Debug.LogWarning("Hazard Prefab is missing!");
            return;
        }

        float randomX = Random.Range(-spawnWidth, spawnWidth);

        Vector3 spawnPosition = new Vector3(
            transform.position.x + randomX,
            transform.position.y + spawnHeight,
            0f
        );

        Instantiate(
            hazardPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }

    // Denne må KUN findes én gang
    public void StopSpawning()
    {
        phaseFinished = true;

        Debug.Log("Hazard spawning stopped.");
    }
}
