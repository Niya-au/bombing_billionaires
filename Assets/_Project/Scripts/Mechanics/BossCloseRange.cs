using UnityEngine;

public class BossCloseRange : MonoBehaviour
{
    public int damage = 20;
    public float damageCooldown = 0.8f;
    public float knockbackForce = 6f;

    private float nextDamageTime = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Time.time < nextDamageTime)
            return;

        Health playerHealth = other.GetComponent<Health>();
        Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        if (playerRb != null)
        {
            Vector2 direction =
                (other.transform.position - transform.position).normalized;

            playerRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }

        nextDamageTime = Time.time + damageCooldown;
    }
}