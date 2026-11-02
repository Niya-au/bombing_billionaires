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

    [Header("Next Level")]
    [SerializeField] private string nextLevelName = "Level 4";
    [SerializeField] private float nextLevelDelay = 1f;

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
        // Stop spawning once the goal has been reached.
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

        // Add the money.
        collectedMoney += amount;

        Debug.Log(
            "Money collected: $" +
            collectedMoney +
            " / $" +
            collectionGoal
        );

        UpdateMoneyUI();

        // The important part:
        // We DON'T need exactly 1000.
        // 975 + 50 = 1025 -> goal reached.
        if (collectedMoney >= collectionGoal)
        {
            CollectionGoalReached();
        }
    }


    private void UpdateMoneyUI()
    {
        // Update text if you still have the old text.
        if (moneyText != null)
        {
            moneyText.text =
                "Money: $" +
                collectedMoney +
                " / $" +
                collectionGoal;
        }

        // Update the new money bar.
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
        Debug.Log("LOADING NEXT LEVEL...");
        Debug.Log("==============================");

        // Make the bar look completely full.
        if (moneyBar != null)
        {
            moneyBar.SetMoney(
                collectionGoal,
                collectionGoal
            );
        }

        // Load Level 4.
        Invoke(nameof(LoadNextLevel), nextLevelDelay);
    }


    private void LoadNextLevel()
    {
        // If a scene name has been entered, use it.
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            Debug.Log("Loading scene: " + nextLevelName);

            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            // Otherwise load the next scene in Build Settings.
            int nextSceneIndex =
                SceneManager.GetActiveScene().buildIndex + 1;

            Debug.Log(
                "Loading next scene with build index: " +
                nextSceneIndex
            );

            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}