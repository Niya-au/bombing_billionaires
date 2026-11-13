using UnityEngine;

public class BossDamageReceiver : MonoBehaviour
{
    public Health bossHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Boss trigger hit by: " + other.name);

        if (other.CompareTag("PlayerAttack"))
        {
            Debug.Log("Boss takes damage!");
            bossHealth.TakeDamage(3);
        }
    }
}