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

    private void Update()
    {
        // Hazards starter først efter spilleren har samlet $1000
        if (moneySpawner == null || !moneySpawner.GoalReached)
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
}