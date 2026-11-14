using UnityEngine;

public class BossContactDamage : MonoBehaviour
{
    public int damage = 10;
    public float damageCooldown = 1f;

    private float nextDamageTime = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Time.time < nextDamageTime)
            return;

        Health playerHealth = other.GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            nextDamageTime = Time.time + damageCooldown;
        }
    }
}