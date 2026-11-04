using UnityEngine;

public class AlonBlueProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Health playerHealth = other.GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);

            Debug.Log("🔵 ALON PROJECTILE HIT BK! -" + damage + " HP");

            Destroy(gameObject);
        }
    }
}