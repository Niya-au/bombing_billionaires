using UnityEngine;
//ny 

public class MoneyPickup : MonoBehaviour
{
    [SerializeField] private int moneyValue = 100;
    [SerializeField, Min(0f)] private float pickupDelay = 1f;
    private float collectibleAt;

    private void OnEnable()
    {
        collectibleAt = Time.time + pickupDelay;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time < collectibleAt)
            return;

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