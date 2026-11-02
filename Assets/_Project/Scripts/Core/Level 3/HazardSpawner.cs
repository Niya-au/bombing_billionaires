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

    [Tooltip("Hvor langt fra midten hazards må spawne til venstre/højre.")]
    [SerializeField] private float spawnWidth = 8f;

    [Tooltip("Hvor højt over toppen hazards spawner.")]
    [SerializeField] private float spawnAboveScreen = 1f;

    [Tooltip("Hvor hurtigt hazards falder.")]
    [SerializeField] private float hazardFallSpeed = 3.5f;

    [Header("Hazard Sizes")]
    [SerializeField] private float asteroidScale = 1.5f;
    [SerializeField] private float debrisScale = 1.5f;
    [SerializeField] private float laserScale = 0.45f;

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
        if (phaseText != null)
        {
            phaseText.text = "";
        }
    }


    private void Update()
    {
        // Vent på at spilleren når målet
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


        // Bevæg alle hazards nedad
        MoveHazards();


        // Spawn nye hazards
        if (spawnTimer >= spawnInterval)
        {
            SpawnHazard();
            spawnTimer = 0f;
        }


        // Hazard-fasen slutter efter tiden
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


        // Find kameraet
        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("HAZARD ERROR: Main Camera blev ikke fundet!");
            return;
        }


        // Kameraets top
        float topY =
            cam.transform.position.y +
            cam.orthographicSize +
            spawnAboveScreen;


        // X-position indenfor skærmen
        float cameraHalfWidth =
            cam.orthographicSize * cam.aspect;

        float randomX =
            Random.Range(
                -cameraHalfWidth + 0.5f,
                cameraHalfWidth - 0.5f
            );


        // Spawn oven over skærmen
        Vector3 spawnPosition = new Vector3(
            cam.transform.position.x + randomX,
            topY,
            0f
        );


        // Spawn
        GameObject hazard = Instantiate(
            selectedHazard,
            spawnPosition,
            Quaternion.identity
        );


        // Sæt størrelse
        if (selectedHazard == asteroidPrefab)
        {
            hazard.transform.localScale =
                Vector3.one * asteroidScale;
        }
        else if (selectedHazard == debrisPrefab)
        {
            hazard.transform.localScale =
                Vector3.one * debrisScale;
        }
        else if (selectedHazard == laserPrefab)
        {
            hazard.transform.localScale =
                Vector3.one * laserScale;
        }


        // Gem hazard
        spawnedHazards.Add(hazard);


        Debug.Log(
            "HAZARD SPAWNED: " +
            selectedHazard.name
        );
    }


    private void MoveHazards()
    {
        for (int i = spawnedHazards.Count - 1; i >= 0; i--)
        {
            GameObject hazard = spawnedHazards[i];

            if (hazard == null)
            {
                spawnedHazards.RemoveAt(i);
                continue;
            }


            // Fald ned
            hazard.transform.position +=
                Vector3.down *
                hazardFallSpeed *
                Time.deltaTime;


            // Fjern hazards der er faldet langt under skærmen
            Camera cam = Camera.main;

            if (cam != null)
            {
                float bottomY =
                    cam.transform.position.y -
                    cam.orthographicSize -
                    3f;

                if (hazard.transform.position.y < bottomY)
                {
                    Destroy(hazard);
                    spawnedHazards.RemoveAt(i);
                }
            }
        }
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


        ClearHazards();


        if (phaseText != null)
        {
            phaseText.text =
                "HAZARD PHASE COMPLETE!\n" +
                "GET READY...";
        }


        // Start Alon boss fight
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


        OnHazardPhaseComplete?.Invoke();
    }


    private void PlayerDied()
    {
        phaseFinished = true;

        Debug.Log(
            "PLAYER DIED DURING HAZARD PHASE!"
        );


        ClearHazards();


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