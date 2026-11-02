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

    [Header("Boss")]
    [SerializeField] private GameObject alonBoss;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 0.7f;
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

        // Alon skal være skjult ved spilstart
        if (alonBoss != null)
        {
            alonBoss.SetActive(false);
        }

        Debug.Log("HAZARD SPAWNER READY");


        // Advarsler hvis noget mangler
        if (asteroidPrefab == null)
        {
            Debug.LogWarning("HAZARD WARNING: Asteroid Prefab mangler!");
        }

        if (debrisPrefab == null)
        {
            Debug.LogWarning("HAZARD WARNING: Debris Prefab mangler!");
        }

        if (laserPrefab == null)
        {
            Debug.LogWarning("HAZARD WARNING: Laser Prefab mangler!");
        }

        if (moneySpawner == null)
        {
            Debug.LogError("HAZARD ERROR: MoneySpawner blev ikke fundet!");
        }

        if (playerHealth == null)
        {
            Debug.LogWarning("HAZARD WARNING: Health blev ikke fundet!");
        }

        if (alonBoss == null)
        {
            Debug.LogWarning("BOSS WARNING: Alon Boss mangler!");
        }
    }


    private void Start()
    {
        // Skjul phase text i starten
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


        // Hvis hazard-fasen er færdig
        if (phaseFinished)
        {
            return;
        }


        // Hvis BK er død
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
        // Lav liste med de hazards der er sat i Inspector
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


        // Hvis ingen hazards findes
        if (availableHazards.Count == 0)
        {
            Debug.LogError(
                "CANNOT SPAWN HAZARD: Ingen hazard prefabs er sat!"
            );

            return;
        }


        // Vælg tilfældig hazard
        GameObject selectedHazard =
            availableHazards[
                Random.Range(0, availableHazards.Count)
            ];


        // Tilfældig X-position
        float randomX =
            Random.Range(-spawnWidth, spawnWidth);


        // Spawn-position
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


        // Gem hazard
        spawnedHazards.Add(hazard);


        Debug.Log(
            "HAZARD SPAWNED: " +
            selectedHazard.name
        );
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
                "GET READY...";
        }


        // ==============================
        // START ALON THRUSK BOSS FIGHT
        // ==============================

        if (alonBoss != null)
        {
            // Gør Alon synlig
            alonBoss.SetActive(true);


            // Find Alon-scriptet
            AlonThruskBoss boss =
                alonBoss.GetComponent<AlonThruskBoss>();


            // Start boss fight
            if (boss != null)
            {
                boss.StartBossFight();
            }
            else
            {
                Debug.LogError(
                    "BOSS ERROR: AlonThruskBoss script mangler på alon_0!"
                );
            }


            Debug.Log("==============================");
            Debug.Log("ALON THRUSK HAS ARRIVED!");
            Debug.Log("BOSS FIGHT STARTED!");
            Debug.Log("==============================");
        }
        else
        {
            Debug.LogError(
                "BOSS ERROR: Alon Boss er ikke sat!"
            );
        }


        // Send event
        OnHazardPhaseComplete?.Invoke();
    }


    private void PlayerDied()
    {
        phaseFinished = true;

        Debug.Log(
            "PLAYER DIED DURING HAZARD PHASE!"
        );


        // Fjern hazards
        ClearHazards();


        // Alon skal ikke starte hvis BK dør
        if (alonBoss != null)
        {
            alonBoss.SetActive(false);
        }


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