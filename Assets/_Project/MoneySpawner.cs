using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    [Header("Money")]
    [SerializeField] private GameObject moneyPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnWidth = 8f;

    [Header("Collection Goal")]
    [SerializeField] private int collectionGoal = 1000;

    private int collectedMoney = 0;
    private float spawnTimer;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnMoney();
            spawnTimer = 0f;
        }
    }

    private void SpawnMoney()
    {
        float randomX = Random.Range(-spawnWidth, spawnWidth);

        Vector3 spawnPosition = new Vector3(
            transform.position.x + randomX,
            transform.position.y,
            transform.position.z
        );

        Instantiate(moneyPrefab, spawnPosition, Quaternion.identity);
    }

    public void CollectMoney(int amount)
    {
        collectedMoney += amount;

        Debug.Log("Money collected: $" + collectedMoney +
                  " / $" + collectionGoal);

        if (collectedMoney >= collectionGoal)
        {
            CollectionGoalReached();
        }
    }

    private void CollectionGoalReached()
    {
        Debug.Log("Collection goal reached!");

        // Continue to the next phase or level here.
    }
}