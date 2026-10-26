 
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

    [Header("Level 3")]
    [SerializeField] private MoneySpawner moneySpawner;
    [SerializeField] private Health playerHealth;

    private float spawnTimer;
    private float hazardTimer;

    private bool phaseStarted;
    private bool phaseFinished;

    public float TimeRemaining
    {
        get
        {
            return Mathf.Max(0f, hazardDuration - hazardTimer);
        }
    }

    public bool PhaseFinished => phaseFinished;

    private void Update()
    {
        // Vent på at $1000 er samlet
        if (moneySpawner == null)
            return;

        if (!moneySpawner.GoalReached)
            return;

        // Start hazard phase
        if (!phaseStarted)
        {
            phaseStarted = true;
            phaseFinished = false;

            spawnTimer = 0f;
            hazardTimer = 0f;

            Debug.Log("================================");
            Debug.Log("HAZARD PHASE STARTED!");
            Debug.Log("================================");
        }

        // Stop hvis fasen er færdig
        if (phaseFinished)
            return;

        // Stop hvis spilleren er død
        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            StopSpawning();
            return;
        }

        // Timer
        hazardTimer += Time.deltaTime;

        // Spawn timer
        spawnTimer += Time.deltaTime;

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
            Debug.LogError("HAZARD PREFAB MANGLER!");
            return;
        }

        // Spawn direkte i den synlige del af banen
        float randomX = Random.Range(-8f, 8f);

        Vector3 spawnPosition = new Vector3(
            randomX,
            spawnY,
            0f
        );

        GameObject hazard = Instantiate(
            hazardPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Debug.Log(
            "HAZARD SPAWNED! Position: " + spawnPosition
        );
    }

    public void StopSpawning()
    {
        phaseFinished = true;

        Debug.Log("HAZARD SPAWNING STOPPED!");
    }

    private void FinishHazardPhase()
    {
        phaseFinished = true;

        Debug.Log("==============================");
        Debug.Log("HAZARD PHASE COMPLETE!");
        Debug.Log("PLAYER SURVIVED 15 SECONDS!");
        Debug.Log("==============================");
    }
}