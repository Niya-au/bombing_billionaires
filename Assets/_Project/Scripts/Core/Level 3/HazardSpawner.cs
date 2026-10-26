
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

    // Bruges af andre scripts
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
        // Vent på $1000
        if (moneySpawner == null)
            return;

        if (!moneySpawner.GoalReached)
            return;

        // Start hazard-fasen
        if (!phaseStarted)
        {
            StartHazardPhase();
        }

        // Fasen er færdig
        if (phaseFinished)
            return;

        // Hvis spilleren dør
        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            StopSpawning();

            Debug.Log("PLAYER DIED DURING HAZARD PHASE!");

            return;
        }

        // Tæl tiden
        hazardTimer += Time.deltaTime;

        // Tæl spawn timer
        spawnTimer += Time.deltaTime;

        // Spawn hazard
        if (spawnTimer >= spawnInterval)
        {
            SpawnHazard();
            spawnTimer = 0f;
        }

        // 15 sekunder er gået
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

        // Her kan vi senere starte næste del af Level 3.
    }
}