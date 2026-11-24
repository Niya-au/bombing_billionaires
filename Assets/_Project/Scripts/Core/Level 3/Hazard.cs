
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 10;

    private bool hasHitPlayer = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitPlayer)
            return;

        if (!other.CompareTag("Player"))
            return;

        Health health = other.GetComponentInParent<Health>();

        if (health != null)
        {
            hasHitPlayer = true;

            health.TakeDamage(damage);

            Debug.Log(
                "Hazard hit BK! -" + damage + " HP"
            );

            Destroy(gameObject);
        }
    }
}
