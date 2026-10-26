using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

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

    [Header("UI")]
    [SerializeField] private TMP_Text phaseText;

    [Header("Phase Complete")]
    public UnityEvent OnHazardPhaseComplete;

    private float spawnTimer;
    private float hazardTimer;

    private bool phaseStarted;
    private bool phaseFinished;

    // Holder styr på alle hazards vi selv har spawned
    private List<GameObject> spawnedHazards = new List<GameObject>();

    private void Start()
    {
        // Skjul teksten når spillet starter
        if (phaseText != null)
        {
            phaseText.text = "";
        }
    }

    private void Update()
    {
        // Vent på $1000
        if (moneySpawner == null)
            return;

        if (!moneySpawner.GoalReached)
            return;

        // Start fasen
        if (!phaseStarted)
        {
            StartHazardPhase();
        }

        // Hvis fasen er færdig, gør ingenting
        if (phaseFinished)
            return;

        // Hvis spilleren dør
        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            StopSpawning();

            if (phaseText != null)
            {
                phaseText.text = "YOU DIED!";
            }

            return;
        }

        // Timer
        hazardTimer += Time.deltaTime;
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
        phaseFinished = false;

        hazardTimer = 0f;
        spawnTimer = 0f;

        Debug.Log("================================");
        Debug.Log("HAZARD PHASE STARTED!");
        Debug.Log("SURVIVE FOR " + hazardDuration + " SECONDS!");
        Debug.Log("================================");

        if (phaseText != null)
        {
            phaseText.text =
                "HAZARD PHASE\n" +
                "SURVIVE " + hazardDuration + " SECONDS!";
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

        GameObject newHazard = Instantiate(
            hazardPrefab,
            spawnPosition,
            Quaternion.identity
        );

        // Gem hazarden i vores liste
        spawnedHazards.Add(newHazard);

        Debug.Log("HAZARD SPAWNED!");
    }

    public void StopSpawning()
    {
        phaseFinished = true;

        Debug.Log("Hazard spawning stopped.");

        // Fjern alle hazards der stadig eksisterer
        ClearAllHazards();
    }

    private void ClearAllHazards()
    {
        for (int i = spawnedHazards.Count - 1; i >= 0; i--)
        {
            if (spawnedHazards[i] != null)
            {
                Destroy(spawnedHazards[i]);
            }
        }

        spawnedHazards.Clear();

        Debug.Log("All remaining hazards destroyed.");
    }

    private void FinishHazardPhase()
    {
        phaseFinished = true;

        // STOP spawning
        StopSpawning();

        Debug.Log("================================");
        Debug.Log("PLAYER SURVIVED 15 SECONDS!");
        Debug.Log("HAZARD PHASE COMPLETE!");
        Debug.Log("================================");

        // Tekst på skærmen
        if (phaseText != null)
        {
            phaseText.text =
                "HAZARD PHASE COMPLETE!\n" +
                "YOU SURVIVED!";
        }

        // Fortæl andre scripts at fasen er færdig
        OnHazardPhaseComplete?.Invoke();
    }
}