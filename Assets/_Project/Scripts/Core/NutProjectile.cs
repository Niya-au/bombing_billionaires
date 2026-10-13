
using UnityEngine;

public class NutProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float lifetime = 5f;
    public int damage = 10;

    void Start()
    {
        // Destroy the nut after 5 seconds.
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);

                Debug.Log("Player HP: " + playerHealth.currentHP);
            }

            Destroy(gameObject);
        }

        else if (other.gameObject.layer ==
                 LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}

