
using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [Header("Hazard")]
    [SerializeField] private GameObject hazardPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnWidth = 8f;
    [SerializeField] private float spawnY = 5f;

    [Header("Hazard Phase")]
    [SerializeField] private float hazardDuration = 15f;

    private MoneySpawner moneySpawner;
    private Health playerHealth;

    private float spawnTimer = 0f;
    private float hazardTimer = 0f;

    private bool phaseStarted = false;
    private bool phaseFinished = false;

    public float TimeRemaining =>
        Mathf.Max(0f, hazardDuration - hazardTimer);

    public bool PhaseFinished => phaseFinished;

    private void Start()
    {
        // Find automatisk
        moneySpawner = FindFirstObjectByType<MoneySpawner>();
        playerHealth = FindFirstObjectByType<Health>();

        Debug.Log("HAZARD SPAWNER READY");

        if (moneySpawner == null)
            Debug.LogError("HAZARD ERROR: MoneySpawner blev ikke fundet!");

        if (playerHealth == null)
            Debug.LogError("HAZARD ERROR: Health blev ikke fundet!");

        if (hazardPrefab == null)
            Debug.LogError("HAZARD ERROR: Hazard Prefab mangler!");
    }

    private void Update()
    {
        // Hvis MoneySpawner ikke blev fundet
        if (moneySpawner == null)
            return;

        // Vent på $1000
        if (!moneySpawner.GoalReached)
            return;

        // Start fasen
        if (!phaseStarted)
        {
            phaseStarted = true;
            hazardTimer = 0f;
            spawnTimer = 0f;

            Debug.Log("================================");
            Debug.Log("HAZARD PHASE STARTED!");
            Debug.Log("SURVIVE FOR 15 SECONDS!");
            Debug.Log("================================");
        }

        // Fasen er færdig
        if (phaseFinished)
            return;

        // Spilleren er død
        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            StopSpawning();
            return;
        }

        // Timer
        hazardTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        // Spawn
        if (spawnTimer >= spawnInterval)
        {
            SpawnHazard();
            spawnTimer = 0f;
        }

        // 15 sekunder
        if (hazardTimer >= hazardDuration)
        {
            FinishHazardPhase();
        }
    }

    private void SpawnHazard()
    {
        if (hazardPrefab == null)
        {
            Debug.LogError("KAN IKKE SPAWNE: Hazard Prefab mangler!");
            return;
        }

        float randomX = Random.Range(-spawnWidth, spawnWidth);

        Vector3 spawnPosition = new Vector3(
            randomX,
            spawnY,
            0f
        );

        Instantiate(
            hazardPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Debug.Log("HAZARD SPAWNED!");
    }

    public void StopSpawning()
    {
        phaseFinished = true;

        Debug.Log("HAZARD SPAWNING STOPPED!");
    }

    private void FinishHazardPhase()
    {
        phaseFinished = true;

        Debug.Log("================================");
        Debug.Log("HAZARD PHASE COMPLETE!");
        Debug.Log("PLAYER SURVIVED 15 SECONDS!");
        Debug.Log("================================");
    }
}
