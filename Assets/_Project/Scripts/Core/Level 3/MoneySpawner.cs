
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

    [Header("UI")]
    [SerializeField] private MoneyBar moneyBar;
    [SerializeField] private TMP_Text moneyText;

    private int collectedMoney = 0;
    private float spawnTimer = 0f;
    private bool levelCompleted = false;

    public int CollectedMoney => collectedMoney;

    public bool GoalReached => collectedMoney >= collectionGoal;


    private void Start()
    {
        collectedMoney = 0;
        levelCompleted = false;

        UpdateMoneyUI();

        if (moneyBar != null)
        {
            moneyBar.SetMoney(0, collectionGoal);
            moneyBar.gameObject.SetActive(true);
        }
    }


    private void Update()
    {
        if (levelCompleted)
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
            Debug.LogError("MoneySpawner: No money prefabs assigned!");
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

        if (randomMoney != null)
        {
            Instantiate(
                randomMoney,
                spawnPosition,
                Quaternion.identity
            );
        }
    }


    public void CollectMoney(int amount)
    {
        if (levelCompleted)
            return;

        if (amount <= 0)
            return;

        collectedMoney += amount;

        Debug.Log(
            "Money collected: $" +
            collectedMoney +
            " / $" +
            collectionGoal
        );

        UpdateMoneyUI();

        // Det er IKKE nødvendigt at ramme præcis $1000.
        // Fx 975 + 50 = 1025 → bossfasen starter.
        if (collectedMoney >= collectionGoal)
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

        if (moneyBar != null)
        {
            moneyBar.SetMoney(
                collectedMoney,
                collectionGoal
            );
        }
    }


    private void CollectionGoalReached()
    {
        if (levelCompleted)
            return;

        levelCompleted = true;

        Debug.Log("==============================");
        Debug.Log(
            "MONEY GOAL REACHED! $" +
            collectedMoney
        );
        Debug.Log("STARTING BOSS PHASE!");
        Debug.Log("==============================");

        // Fyld baren helt.
        if (moneyBar != null)
        {
            moneyBar.SetMoney(
                collectionGoal,
                collectionGoal
            );

            // MoneyBar skal væk under bosskampen.
            moneyBar.gameObject.SetActive(false);
        }

        // Gammel Money Text skal også væk.
        if (moneyText != null)
        {
            moneyText.gameObject.SetActive(false);
        }

        // VIGTIGT:
        // Vi loader IKKE Level 4.
        // HazardSpawner opdager GoalReached
        // og starter bossfasen.
    }
}