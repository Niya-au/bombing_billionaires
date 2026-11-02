using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        }
    }


    private void Update()
    {
        // Stop spawning money when the goal is reached.
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
        // Don't collect more after the goal has been reached.
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

        // It does NOT have to be exactly $1000.
        // Example:
        // $975 + $50 = $1025 -> goal reached.
        if (collectedMoney >= collectionGoal)
        {
            CollectionGoalReached();
        }
    }


    private void UpdateMoneyUI()
    {
        // Old money text, if you still use it.
        if (moneyText != null)
        {
            moneyText.text =
                "Money: $" +
                collectedMoney +
                " / $" +
                collectionGoal;
        }

        // New MoneyBar.
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
        Debug.Log("STARTING HAZARD PHASE!");
        Debug.Log("==============================");

        // Make the bar visually full.
        if (moneyBar != null)
        {
            moneyBar.SetMoney(
                collectionGoal,
                collectionGoal
            );
        }

        // IMPORTANT:
        // We do NOT load another scene here.
        //
        // HazardSpawner is watching GoalReached.
        // When GoalReached becomes true,
        // HazardSpawner will start the asteroid/debris/laser phase.
    }
}