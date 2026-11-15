using System.Collections;
using UnityEngine;

public class BossCloseRange : MonoBehaviour
{
    public int damage = 35;
    public float damageCooldown = 0.25f;
    public float knockbackForce = 15f;
    public float knockbackStun = 0.25f;

    private float nextDamageTime = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Time.time < nextDamageTime)
            return;

        Health playerHealth = other.GetComponent<Health>();
        Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
        TestPlayerMovement playerMovement = other.GetComponent<TestPlayerMovement>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        if (playerRb != null)
        {
            Vector2 direction =
                (other.transform.position - transform.position).normalized;

            playerRb.linearVelocity = direction * knockbackForce;
        }

        if (playerMovement != null)
        {
            StartCoroutine(KnockbackStun(playerMovement));
        }

        nextDamageTime = Time.time + damageCooldown;
    }

    IEnumerator KnockbackStun(TestPlayerMovement playerMovement)
    {
        playerMovement.enabled = false;

        yield return new WaitForSeconds(knockbackStun);

        playerMovement.enabled = true;
    }
}