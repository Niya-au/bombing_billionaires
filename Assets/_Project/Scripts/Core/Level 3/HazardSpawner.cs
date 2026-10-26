
using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [Header("Hazard")]
    [SerializeField] private GameObject hazardPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnWidth = 8f;
    [SerializeField] private float spawnHeight = 6f;

    [Header("Hazard Phase")]
    [SerializeField] private float hazardDuration = 15f;

    [Header("Level 3")]
    [SerializeField] private MoneySpawner moneySpawner;
    [SerializeField] private Health playerHealth;

    private float spawnTimer = 0f;
    private float hazardTimer = 0f;

    private bool phaseStarted = false;
    private bool phaseFinished = false;

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
        if (moneySpawner == null)
        {
            Debug.LogWarning("HazardSpawner: MoneySpawner mangler!");
            return;
        }

        // Vent på $1000
        if (!moneySpawner.GoalReached)
            return;

        // Start hazard-fasen
        if (!phaseStarted)
        {
            StartHazardPhase();
        }

        // Stop når fasen er færdig
        if (phaseFinished)
            return;

        // Stop hvis spilleren er død
        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            StopSpawning();

            Debug.Log("PLAYER DIED DURING HAZARD PHASE!");
            return;
        }

        // Hazard timer
        hazardTimer += Time.deltaTime;

        // Spawn timer
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnHazard();
            spawnTimer = 0f;
        }

        // 15 sekunder gået
        if (hazardTimer >= hazardDuration)
        {
            FinishHazardPhase();
        }
    }

    private void StartHazardPhase()
    {
        phaseStarted = true;

        hazardTimer = 0f;
        spawnTimer = 0f;

        Debug.Log("HAZARD PHASE STARTED!");
        Debug.Log("SURVIVE FOR " + hazardDuration + " SECONDS!");
    }

    private void SpawnHazard()
    {
        if (hazardPrefab == null)
        {
            Debug.LogError("HazardSpawner: Hazard Prefab mangler!");
            return;
        }

        float randomX = Random.Range(-spawnWidth, spawnWidth);

        // VIGTIGT:
        // spawnHeight er den faktiske Y-position.
        Vector3 spawnPosition = new Vector3(
            transform.position.x + randomX,
            spawnHeight,
            0f
        );

        GameObject newHazard = Instantiate(
            hazardPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Debug.Log("HAZARD SPAWNED AT: " + spawnPosition);
    }

    public void StopSpawning()
    {
        phaseFinished = true;

        Debug.Log("Hazard spawning stopped.");
    }

    private void FinishHazardPhase()
    {
        phaseFinished = true;

        Debug.Log("HAZARD PHASE COMPLETE!");
        Debug.Log("PLAYER SURVIVED " + hazardDuration + " SECONDS!");

        // Her starter vi næste del af Level 3 senere.
    }
}