
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoneySpawner : MonoBehaviour
{
    [Header("Money")]
    [SerializeField] private GameObject[] moneyPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnWidth = 8f;

    [Header("Collection Goal")]
    [SerializeField] private int collectionGoal = 1000;

    [Header("UI")]
    [SerializeField] private Slider moneyBar;
    [SerializeField] private TMP_Text moneyText;

    [Header("Money Bar Animation")]
    [SerializeField] private float barSpeed = 500f;

    private int collectedMoney = 0;
    private float spawnTimer = 0f;

    public int CollectedMoney => collectedMoney;
    public bool GoalReached => collectedMoney >= collectionGoal;


    private void Start()
    {
        // Money bar går lidt over målet
        if (moneyBar != null)
        {
            moneyBar.minValue = 0;
            moneyBar.maxValue = collectionGoal + 200;
            moneyBar.value = 0;
        }

        UpdateMoneyUI();
    }


    private void Update()
    {
        // Spawn penge indtil målet er nået
        if (!GoalReached)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                SpawnMoney();
                spawnTimer = 0f;
            }
        }

        // Money bar bevæger sig gradvist mod det aktuelle beløb
        if (moneyBar != null)
        {
            moneyBar.value = Mathf.MoveTowards(
                moneyBar.value,
                collectedMoney,
                barSpeed * Time.deltaTime
            );
        }
    }


    private void SpawnMoney()
    {
        if (moneyPrefabs == null || moneyPrefabs.Length == 0)
        {
            Debug.LogError("No money prefabs assigned!");
            return;
        }

        float randomX = Random.Range(
            -spawnWidth,
            spawnWidth
        );

        Vector3 spawnPosition = new Vector3(
            transform.position.x + randomX,
            transform.position.y,
            transform.position.z
        );

        GameObject randomMoney =
            moneyPrefabs[
                Random.Range(0, moneyPrefabs.Length)
            ];

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

        if (GoalReached)
        {
            CollectionGoalReached();
        }
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


    private void CollectionGoalReached()
    {
        Debug.Log("Collection goal reached!");
    }
}