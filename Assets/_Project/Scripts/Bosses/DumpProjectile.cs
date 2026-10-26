using UnityEngine;

public class DumpProjectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float hitDistance = 0.8f;

    private Transform player;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (player == null)
            return;

        // Flyv direkte mod spilleren
        Vector2 direction =
            (player.position - transform.position).normalized;

        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);

        // Ekstra sikkerhed:
        // Hvis vi er tæt nok på spilleren, tæller det som et hit.
        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance <= hitDistance)
        {
            HitPlayer();
        }
    }

    private void HitPlayer()
    {
        Health health = player.GetComponent<Health>();

        if (health == null)
        {
            health = player.GetComponentInParent<Health>();
        }

        if (health != null)
        {
            Debug.Log("💥 HAZARD HIT PLAYER! -" + damage + " HP");

            health.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning("Player has no Health component!");
        }

        Destroy(gameObject);
    }
}