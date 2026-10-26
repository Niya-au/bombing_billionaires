
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

    public bool PhaseStarted => phaseStarted;
    public bool PhaseFinished => phaseFinished;

    private void Update()
    {
        // Vent på at spilleren har samlet $1000
        if (moneySpawner == null)
            return;

        if (!moneySpawner.GoalReached)
            return;

        // Start hazard-fasen én gang
        if (!phaseStarted)
        {
            StartHazardPhase();
        }

        // Fasen er færdig
        if (phaseFinished)
            return;

        // Hvis spilleren er død
        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            StopSpawning();

            Debug.Log("Player died during hazard phase.");

            return;
        }

        // Tæl tiden i hazard-fasen
        hazardTimer += Time.deltaTime;

        // Spawn hazards
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnHazard();
            spawnTimer = 0f;
        }

        // Efter 15 sekunder
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

    // Denne funktion kan kaldes fra HazardPhase
    public void StopSpawning()
    {
        phaseFinished = true;

        Debug.Log("Hazard spawning stopped.");
    }

    private void FinishHazardPhase()
    {
        phaseFinished = true;

        Debug.Log("HAZARD PHASE COMPLETE!");
        Debug.Log("Player survived 15 seconds!");

        // HazardPhase kan nu fortsætte til næste del:
        // BOSS FIGHT
    }
    public void StopSpawning()
{
    phaseFinished = true;

    Debug.Log("Hazard spawning stopped.");
}}