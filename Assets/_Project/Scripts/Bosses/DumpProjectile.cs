using UnityEngine;

public class DumpProjectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float hitDistance = 0.8f;

    private Transform player;
    private Vector2 direction = Vector2.left;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        Destroy(gameObject, lifetime);
    }

    // Denne skal være her, fordi DumpBoss bruger den
    public void SetDirection(Vector2 newDirection)
    {
        if (newDirection.sqrMagnitude > 0.001f)
        {
            direction = newDirection.normalized;
        }
    }

    private void Update()
    {
        // Hvis der findes en spiller, så flyv mod spilleren
        if (player != null)
        {
            direction = (player.position - transform.position).normalized;
        }

        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);

        // Sikkerheds-hit hvis projektilet kommer tæt nok på
        if (player != null)
        {
            float distance = Vector2.Distance(
                transform.position,
                player.position
            );

            if (distance <= hitDistance)
            {
                HitPlayer();
            }
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