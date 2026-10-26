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

    public UnityEvent OnHazardPhaseComplete;

    private float spawnTimer;
    private float hazardTimer;

    private bool phaseStarted;
    private bool phaseFinished;

    private List<GameObject> spawnedHazards = new List<GameObject>();


    private void Awake()
    {
        // Find automatisk hvis Inspector-felterne ikke er sat
        if (moneySpawner == null)
        {
            moneySpawner = FindFirstObjectByType<MoneySpawner>();
        }

        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<Health>();
        }

        if (phaseText == null)
        {
            GameObject phaseObject = GameObject.Find("Phase Text");

            if (phaseObject != null)
            {
                phaseText = phaseObject.GetComponent<TMP_Text>();
            }
        }

        Debug.Log("=== HAZARD SPAWNER READY ===");

        if (hazardPrefab == null)
            Debug.LogError("HAZARD ERROR: Hazard Prefab mangler!");

        if (moneySpawner == null)
            Debug.LogError("HAZARD ERROR: MoneySpawner blev ikke fundet!");

        if (playerHealth == null)
            Debug.LogWarning("HAZARD WARNING: Health blev ikke fundet!");

        if (phaseText == null)
            Debug.LogWarning("HAZARD WARNING: Phase Text blev ikke fundet!");
    }


    private void Start()
    {
        if (phaseText != null)
        {
            phaseText.text = "";
        }
    }


    private void Update()
    {
        // --------------------------------
        // VENT PÅ $1000
        // --------------------------------

        if (moneySpawner == null)
        {
            return;
        }

        if (!moneySpawner.GoalReached)
        {
            return;
        }


        // --------------------------------
        // START HAZARD PHASE
        // --------------------------------

        if (!phaseStarted)
        {
            StartHazardPhase();
        }


        // --------------------------------
        // FASEN ER FÆRDIG
        // --------------------------------

        if (phaseFinished)
        {
            return;
        }


        // --------------------------------
        // PLAYER DØD
        // --------------------------------

        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            PlayerDied();
            return;
        }


        // --------------------------------
        // TIMER
        // --------------------------------

        hazardTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;


        // --------------------------------
        // SPAWN HAZARD
        // --------------------------------

        if (spawnTimer >= spawnInterval)
        {
            SpawnHazard();

            spawnTimer = 0f;
        }


        // --------------------------------
        // 15 SEKUNDER
        // --------------------------------

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
        Debug.Log("SURVIVE FOR 15 SECONDS!");
        Debug.Log("================================");


        if (phaseText != null)
        {
            phaseText.text =
                "HAZARD PHASE\n" +
                "SURVIVE 15 SECONDS!";
        }
    }


    private void SpawnHazard()
    {
        if (hazardPrefab == null)
        {
            Debug.LogError("Cannot spawn hazard - Hazard Prefab is missing!");
            return;
        }


        float randomX = Random.Range(
            -spawnWidth,
            spawnWidth
        );


        Vector3 spawnPosition = new Vector3(
            transform.position.x + randomX,
            transform.position.y + spawnHeight,
            0f
        );


        GameObject hazard = Instantiate(
            hazardPrefab,
            spawnPosition,
            Quaternion.identity
        );


        spawnedHazards.Add(hazard);


        Debug.Log("HAZARD SPAWNED at " + spawnPosition);
    }


    private void FinishHazardPhase()
    {
        if (phaseFinished)
            return;

        phaseFinished = true;


        Debug.Log("================================");
        Debug.Log("PLAYER SURVIVED 15 SECONDS!");
        Debug.Log("HAZARD PHASE COMPLETE!");
        Debug.Log("================================");


        // Stop alle eksisterende hazards
        ClearHazards();


        if (phaseText != null)
        {
            phaseText.text =
                "HAZARD PHASE COMPLETE!\n" +
                "YOU SURVIVED!";
        }


        // Send event videre til næste del
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

        Debug.Log("All hazards cleared.");
    }
}