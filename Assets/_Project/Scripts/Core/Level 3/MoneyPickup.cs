using UnityEngine;
//ny 

public class MoneyPickup : MonoBehaviour
{
    [SerializeField] private int moneyValue = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MoneySpawner spawner = FindFirstObjectByType<MoneySpawner>();

            if (spawner != null)
            {
                spawner.CollectMoney(moneyValue);
            }

            Destroy(gameObject);
        }
    }
}