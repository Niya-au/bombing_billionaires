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

    [Header("Target")]
    [SerializeField] private Transform player;

    private float spawnTimer = 0f;

    private void Start()
    {
        // Find player automatically hvis den ikke er sat i Inspector
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void Update()
    {
        // Start først hazards efter $1000
        if (moneySpawner == null || !moneySpawner.GoalReached)
            return;

        if (hazardPrefab == null || player == null)
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

        GameObject hazard = Instantiate(
            hazardPrefab,
            spawnPosition,
            Quaternion.identity
        );

        DumpProjectile projectile = hazard.GetComponent<DumpProjectile>();

        if (projectile != null)
        {
            Vector2 direction =
                (player.position - spawnPosition).normalized;

            projectile.SetDirection(direction);
        }
    }
}