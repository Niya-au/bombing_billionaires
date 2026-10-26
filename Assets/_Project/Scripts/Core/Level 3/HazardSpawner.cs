using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class HazardSpawner : MonoBehaviour
{
    [Header("Hazards")]
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private GameObject debrisPrefab;
    [SerializeField] private GameObject laserPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1.2f;
    [SerializeField] private float spawnWidth = 8f;
    [SerializeField] private float spawnHeight = 6f;

    [Header("Hazard Phase")]
    [SerializeField] private float hazardDuration = 15f;

    [Header("Level 3")]
    [SerializeField] private MoneySpawner moneySpawner;
    [SerializeField] private Health playerHealth;

    [Header("UI")]
    [SerializeField] private TMP_Text phaseText;

    [Header("Events")]
    public UnityEvent OnHazardPhaseComplete;

    private float spawnTimer = 0f;
    private float hazardTimer = 0f;

    private bool phaseStarted = false;
    private bool phaseFinished = false;

    private List<GameObject> spawnedHazards = new List<GameObject>();


    private void Awake()
    {
        // Find MoneySpawner automatisk
        if (moneySpawner == null)
        {
            moneySpawner = FindFirstObjectByType<MoneySpawner>();
        }

        // Find Health automatisk
        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<Health>();
        }

        // Find Phase Text automatisk
        if (phaseText == null)
        {
            GameObject phaseObject = GameObject.Find("Phase Text");

            if (phaseObject != null)
            {
                phaseText = phaseObject.GetComponent<TMP_Text>();
            }
        }

        Debug.Log("HAZARD SPAWNER READY");

        if (asteroidPrefab == null)
        {
            Debug.LogError("HAZARD ERROR: Asteroid Prefab mangler!");
        }

        if (debrisPrefab == null)
        {
            Debug.LogError("HAZARD ERROR: Debris Prefab mangler!");
        }

        if (laserPrefab == null)
        {
            Debug.LogError("HAZARD ERROR: Laser Prefab mangler!");
        }

        if (moneySpawner == null)
        {
            Debug.LogError("HAZARD ERROR: MoneySpawner blev ikke fundet!");
        }

        if (playerHealth == null)
        {
            Debug.LogWarning("HAZARD WARNING: Health blev ikke fundet!");
        }

        if (phaseText == null)
        {
            Debug.LogWarning("HAZARD WARNING: Phase Text blev ikke fundet!");
        }
    }


    private void Start()
    {
        // Hazard tekst skal være skjult i starten
        if (phaseText != null)
        {
            phaseText.text = "";
        }
    }


    private void Update()
    {
        // Vent på at spilleren når $1000
        if (moneySpawner == null)
        {
            return;
        }

        if (!moneySpawner.GoalReached)
        {
            return;
        }

        // Start hazard-fasen
        if (!phaseStarted)
        {
            StartHazardPhase();
        }

        // Hvis fasen er færdig, stop
        if (phaseFinished)
        {
            return;
        }

        // Hvis spilleren er død
        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            PlayerDied();
            return;
        }

        // Timere
        hazardTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        // Spawn hazards
        if (spawnTimer >= spawnInterval)
        {
            SpawnHazard();
            spawnTimer = 0f;
        }

        // Hazard-fasen slutter efter 15 sekunder
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

        Debug.Log("==============================");
        Debug.Log("HAZARD PHASE STARTED!");
        Debug.Log("SURVIVE FOR " + hazardDuration + " SECONDS!");
        Debug.Log("==============================");

        if (phaseText != null)
        {
            phaseText.text =
                "HAZARD PHASE\n" +
                "SURVIVE " + hazardDuration + " SECONDS!";
        }
    }


    private void SpawnHazard()
    {
        // Lav en liste over de hazards der faktisk findes
        List<GameObject> availableHazards = new List<GameObject>();

        if (asteroidPrefab != null)
        {
            availableHazards.Add(asteroidPrefab);
        }

        if (debrisPrefab != null)
        {
            availableHazards.Add(debrisPrefab);
        }

        if (laserPrefab != null)
        {
            availableHazards.Add(laserPrefab);
        }

        // Hvis ingen hazards er sat
        if (availableHazards.Count == 0)
        {
            Debug.LogError("CANNOT SPAWN HAZARD - No hazard prefabs assigned!");
            return;
        }

        // Vælg tilfældigt hazard
        GameObject selectedHazard =
            availableHazards[Random.Range(0, availableHazards.Count)];

        // Tilfældig X-position
        float randomX = Random.Range(-spawnWidth, spawnWidth);

        // Spawn ovenfra
        Vector3 spawnPosition = new Vector3(
            transform.position.x + randomX,
            transform.position.y + spawnHeight,
            0f
        );

        // Spawn hazard
        GameObject hazard = Instantiate(
            selectedHazard,
            spawnPosition,
            Quaternion.identity
        );

        // Gem hazard så vi kan rydde den senere
        spawnedHazards.Add(hazard);

        Debug.Log("HAZARD SPAWNED: " + selectedHazard.name);
    }


    public void StopSpawning()
    {
        phaseFinished = true;

        Debug.Log("HAZARD SPAWNING STOPPED!");

        ClearHazards();
    }


    private void FinishHazardPhase()
    {
        if (phaseFinished)
        {
            return;
        }

        phaseFinished = true;

        Debug.Log("==============================");
        Debug.Log("PLAYER SURVIVED 15 SECONDS!");
        Debug.Log("HAZARD PHASE COMPLETE!");
        Debug.Log("==============================");

        // Fjern alle hazards
        ClearHazards();

        // Vis besked
        if (phaseText != null)
        {
            phaseText.text =
                "HAZARD PHASE COMPLETE!\n" +
                "YOU SURVIVED!";
        }

        // Send event
        OnHazardPhaseComplete?.Invoke();
    }


    private void PlayerDied()
    {
        phaseFinished = true;

        Debug.Log("PLAYER DIED DURING HAZARD PHASE!");

        ClearHazards();

        if (phaseText != null)
        {
            phaseText.text = "YOU DIED!";
        }
    }


    private void ClearHazards()
    {
        for (int i = spawnedHazards.Count - 1; i >= 0; i--)
        {
            if (spawnedHazards[i] != null)
            {
                Destroy(spawnedHazards[i]);
            }
        }

        spawnedHazards.Clear();

        Debug.Log("ALL HAZARDS CLEARED!");
    }
}