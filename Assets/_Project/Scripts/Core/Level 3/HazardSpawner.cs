using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HazardSpawner : MonoBehaviour
{
    [Header("Hazards")]
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private GameObject debrisPrefab;
    [SerializeField] private GameObject laserPrefab;

    [Header("Spawn Settings")]
    [SerializeField, Min(0.05f)] private float spawnInterval = 1.2f;
    [SerializeField, Min(0f)] private float spawnWidth = 8f;
    [SerializeField] private float spawnHeight = 6f;

    [Header("Events")]
    public UnityEvent OnHazardPhaseComplete;

    private float spawnTimer;
    private bool isSpawning;
    private readonly List<GameObject> spawnedHazards = new List<GameObject>();

    private void Update()
    {
        if (!isSpawning) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnHazard();
        }
    }

    public void StartSpawning()
    {
        spawnTimer = 0f;
        isSpawning = true;
    }

    public void StopSpawning()
    {
        isSpawning = false;
        ClearHazards();
    }

    private void SpawnHazard()
    {
        var available = new List<GameObject>();
        if (asteroidPrefab != null) available.Add(asteroidPrefab);
        if (debrisPrefab != null) available.Add(debrisPrefab);
        if (laserPrefab != null) available.Add(laserPrefab);

        if (available.Count == 0)
        {
            Debug.LogWarning("HazardSpawner: Assign at least one hazard prefab.", this);
            isSpawning = false;
            return;
        }

        GameObject prefab = available[Random.Range(0, available.Count)];
        Vector3 position = transform.position +
                           new Vector3(Random.Range(-spawnWidth, spawnWidth), spawnHeight, 0f);
        GameObject hazard = Instantiate(prefab, position, Quaternion.identity);
        spawnedHazards.Add(hazard);
    }

    private void ClearHazards()
    {
        foreach (GameObject hazard in spawnedHazards)
            if (hazard != null) Destroy(hazard);
        spawnedHazards.Clear();
    }

    private bool bossDefeatHandled;

    public void BossDefeated()
    {
        if (bossDefeatHandled)
            return;

        bossDefeatHandled = true;

        StopSpawning();

        Debug.Log("ALON DEFEATED!");
        OnHazardPhaseComplete?.Invoke();
    }
}
