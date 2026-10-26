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
        if (moneySpawner == null)
        {
            Debug.LogWarning("HazardSpawner: MoneySpawner mangler!");
            return;
        }

        // Start først hazards når $1000 er samlet
        if (!moneySpawner.GoalReached)
        {
            return;
        }

        Debug.Log("HAZARDS STARTER!");

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
            transform.position.y + spawnHeight,
            0f
        );

        Debug.Log("Spawning hazard at: " + spawnPosition);

        Instantiate(
            hazardPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}