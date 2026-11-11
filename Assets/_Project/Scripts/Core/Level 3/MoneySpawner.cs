using UnityEngine;
using TMPro;

public class MoneySpawner : MonoBehaviour
{
    [Header("Money")]
    [SerializeField] private GameObject[] moneyPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnWidth = 8f;

    [Header("Collection Goal")]
    [SerializeField] private int collectionGoal = 1000;
    [SerializeField] private TMP_Text moneyText;

    private int collectedMoney = 0;
    private float spawnTimer = 0f;

    public int CollectedMoney => collectedMoney;
    public bool GoalReached => collectedMoney >= collectionGoal;

    private void Update()
    {
        if (GoalReached)
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

        float randomX = Random.Range(-spawnWidth, spawnWidth);

        Vector3 spawnPosition = new Vector3(
            transform.position.x + randomX,
            transform.position.y,
            transform.position.z
        );

        GameObject randomMoney =
            moneyPrefabs[Random.Range(0, moneyPrefabs.Length)];

        Instantiate(randomMoney, spawnPosition, Quaternion.identity);
    }

    public void CollectMoney(int amount)
    {
        collectedMoney += amount;

        moneyText.text =
            "Money: $" + collectedMoney + " / $" + collectionGoal;

        Debug.Log(
            "Money collected: $" +
            collectedMoney +
            " / $" +
            collectionGoal
        );

        if (GoalReached)
        {
            CollectionGoalReached();
        }
    }

    private void CollectionGoalReached()
    {
        Debug.Log("Collection goal reached!");
    }
}