
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

    [Header("Level 3")]
    [SerializeField] private MoneySpawner moneySpawner;
    [SerializeField] private Health playerHealth;

    [Header("UI")]
    [SerializeField] private TMP_Text phaseText;

    [Header("Events")]
    public UnityEvent OnHazardPhaseComplete;

    private float spawnTimer = 0f;

    private bool phaseStarted = false;
    private bool phaseFinished = false;

    private List<GameObject> spawnedHazards =
        new List<GameObject>();


    private void Awake()
    {
        if (moneySpawner == null)
        {
            moneySpawner =
                FindFirstObjectByType<MoneySpawner>();
        }

        if (playerHealth == null)
        {
            playerHealth =
                FindFirstObjectByType<Health>();
        }

        if (phaseText == null)
        {
            GameObject phaseObject =
                GameObject.Find("Phase Text");

            if (phaseObject != null)
            {
                phaseText =
                    phaseObject.GetComponent<TMP_Text>();
            }
        }

        // Alon starter skjult.
        if (alonBoss != null)
        {
            alonBoss.SetActive(false);
        }

        Debug.Log("HAZARD SPAWNER READY");


        if (asteroidPrefab == null)
        {
            Debug.LogWarning(
                "HAZARD WARNING: Asteroid Prefab mangler!"
            );
        }

        if (debrisPrefab == null)
        {
            Debug.LogWarning(
                "HAZARD WARNING: Debris Prefab mangler!"
            );
        }

        if (laserPrefab == null)
        {
            Debug.LogWarning(
                "HAZARD WARNING: Laser Prefab mangler!"
            );
        }

        if (moneySpawner == null)
        {
            Debug.LogError(
                "HAZARD ERROR: MoneySpawner blev ikke fundet!"
            );
        }

        if (playerHealth == null)
        {
            Debug.LogWarning(
                "HAZARD WARNING: Health blev ikke fundet!"
            );
        }

        if (alonBoss == null)
        {
            Debug.LogWarning(
                "BOSS WARNING: Alon Boss mangler!"
            );
        }
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
        if (phaseFinished)
            return;

        if (moneySpawner == null)
            return;


        // Vent indtil BK har samlet omkring $1000.
        if (!moneySpawner.GoalReached)
            return;


        // Start boss/hazard-fasen én gang.
        if (!phaseStarted)
        {
            StartHazardPhase();
        }


        // Hvis BK er død.
        if (playerHealth != null &&
            playerHealth.currentHP <= 0)
        {
            PlayerDied();
            return;
        }


        // Fortsæt med at spawn hazards
        // HELE kampen igennem.
        spawnTimer += Time.deltaTime;


        if (spawnTimer >= spawnInterval)
        {
            SpawnHazard();

            spawnTimer = 0f;
        }
    }


    private void StartHazardPhase()
    {
        phaseStarted = true;
        phaseFinished = false;

        spawnTimer = 0f;


        Debug.Log("==============================");
        Debug.Log("BOSS / HAZARD PHASE STARTED!");
        Debug.Log("ALON THRUSK HAS ARRIVED!");
        Debug.Log("==============================");


        if (phaseText != null)
        {
            phaseText.text =
                "ALON THRUSK\n" +
                "FIGHT!";
        }


        // Start Alon.
        if (alonBoss != null)
        {
            alonBoss.SetActive(true);

            AlonThruskBoss boss =
                alonBoss.GetComponent<AlonThruskBoss>();

            if (boss != null)
            {
                boss.StartBossFight();
            }
            else
            {
                Debug.LogError(
                    "BOSS ERROR: " +
                    "AlonThruskBoss script mangler!"
                );
            }
        }
        else
        {
            Debug.LogError(
                "BOSS ERROR: Alon Boss er ikke sat!"
            );
        }
    }


    private void SpawnHazard()
    {
        List<GameObject> availableHazards =
            new List<GameObject>();


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


        if (availableHazards.Count == 0)
        {
            Debug.LogError(
                "CANNOT SPAWN HAZARD: " +
                "Ingen hazard prefabs er sat!"
            );

            return;
        }


        GameObject selectedHazard =
            availableHazards[
                Random.Range(
                    0,
                    availableHazards.Count
                )
            ];


        float randomX =
            Random.Range(
                -spawnWidth,
                spawnWidth
            );


        Vector3 spawnPosition =
            new Vector3(
                transform.position.x + randomX,
                transform.position.y + spawnHeight,
                0f
            );


        GameObject hazard =
            Instantiate(
                selectedHazard,
                spawnPosition,
                Quaternion.identity
            );


        spawnedHazards.Add(hazard);


        Debug.Log(
            "HAZARD SPAWNED: " +
            selectedHazard.name
        );
    }


    public void StopSpawning()
    {
        if (phaseFinished)
            return;

        phaseFinished = true;

        Debug.Log(
            "HAZARD SPAWNING STOPPED!"
        );

        ClearHazards();
    }


    public void BossDefeated()
    {
        if (phaseFinished)
            return;

        phaseFinished = true;

        Debug.Log("==============================");
        Debug.Log("BOSS DEFEATED!");
        Debug.Log("==============================");

        ClearHazards();

        if (phaseText != null)
        {
            phaseText.text =
                "ALON THRUSK DEFEATED!";
        }

        OnHazardPhaseComplete?.Invoke();
    }


    private void PlayerDied()
    {
        phaseFinished = true;

        Debug.Log(
            "PLAYER DIED DURING BOSS FIGHT!"
        );


        ClearHazards();


        if (alonBoss != null)
        {
            AlonThruskBoss boss =
                alonBoss.GetComponent<AlonThruskBoss>();

            if (boss != null)
            {
                boss.StopBossFight();
            }
        }


        if (phaseText != null)
        {
            phaseText.text = "YOU DIED!";
        }
    }


    private void ClearHazards()
    {
        for (
            int i = spawnedHazards.Count - 1;
            i >= 0;
            i--
        )
        {
            if (spawnedHazards[i] != null)
            {
                Destroy(
                    spawnedHazards[i]
                );
            }
        }


        spawnedHazards.Clear();


        Debug.Log(
            "ALL HAZARDS CLEARED!"
        );
    }
}