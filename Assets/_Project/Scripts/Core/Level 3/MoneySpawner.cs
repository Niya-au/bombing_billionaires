
using UnityEngine;
using TMPro;

public class MoneySpawner : MonoBehaviour
{
    [Header("Money Prefabs")]
    [SerializeField] private GameObject[] moneyPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private float screenPadding = 1f;

    [Header("Collection Goal")]
    [SerializeField] private int collectionGoal = 1000;
    [SerializeField] private TMP_Text moneyText;

    private int collectedMoney = 0;
    private float spawnTimer = 0f;
    private bool isSpawning = true;

    public int CollectedMoney => collectedMoney;
    public bool GoalReached => collectedMoney >= collectionGoal;

    private void Start()
    {
        if (gameCamera == null)
            gameCamera = Camera.main;

        UpdateMoneyUI();
    }

    private void Update()
    {
        if (!isSpawning)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnMoney();
            spawnTimer = 0f;
        }
    }

    private void SpawnMoney()
    {
        if (moneyPrefabs == null || moneyPrefabs.Length == 0)
        {
            Debug.LogError("No money prefabs assigned!");
            return;
        }

        if (gameCamera == null)
        {
            Debug.LogError("MoneySpawner: No camera assigned!");
            return;
        }

        float cameraHeight = gameCamera.orthographicSize;
        float cameraWidth = cameraHeight * gameCamera.aspect;

        Vector3 cameraPosition = gameCamera.transform.position;

        float minX = cameraPosition.x - cameraWidth + screenPadding;
        float maxX = cameraPosition.x + cameraWidth - screenPadding;

        float minY = cameraPosition.y - cameraHeight + screenPadding;
        float maxY = cameraPosition.y + cameraHeight - screenPadding;

        if (minX >= maxX || minY >= maxY)
        {
            Debug.LogError("MoneySpawner: Screen padding is too large!");
            return;
        }

        Vector3 spawnPosition = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            0f
        );

        GameObject randomMoney =
            moneyPrefabs[Random.Range(0, moneyPrefabs.Length)];

        Instantiate(
            randomMoney,
            spawnPosition,
            Quaternion.identity
        );
    }

    public void CollectMoney(int amount)
    {
        collectedMoney += amount;

        UpdateMoneyUI();

        Debug.Log(
            "Money collected: $" +
            collectedMoney +
            " / $" +
            collectionGoal
        );
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text =
                "Money: $" +
                collectedMoney +
                " / $" +
                collectionGoal;
        }
    }

    public void StartSpawning()
    {
        isSpawning = true;
        spawnTimer = 0f;
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void SetSpawning(bool shouldSpawn)
    {
        if (shouldSpawn)
        {
            StartSpawning();
        }
        else
        {
            StopSpawning();
        }
    }
}
