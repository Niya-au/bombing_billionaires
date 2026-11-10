
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
    private bool hasHit = false;

    private void Start()
    {
        // Find BK
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Fjern bomben efter 5 sekunder
        Destroy(gameObject, lifetime);
    }

    // DumpBoss bruger denne metode, når bomben affyres
    public void SetDirection(Vector2 newDirection)
    {
        if (newDirection.sqrMagnitude > 0.001f)
        {
            direction = newDirection.normalized;
        }
    }

    private void Update()
    {
        if (hasHit)
            return;

        // Bomben følger BK, selv når han bevæger sig
        if (player != null)
        {
            Vector2 targetPosition = player.position;
            Vector2 currentPosition = transform.position;

            direction =
                (targetPosition - currentPosition).normalized;
        }

        // Bevæg bomben mod BK
        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);

        // Registrer træf, når bomben er tæt på BK
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit || !other.CompareTag("Player"))
            return;

        if (player == null)
            player = other.transform;

        HitPlayer();
    }

    private void HitPlayer()
    {
        if (hasHit)
            return;

        hasHit = true;

        Health health = null;

        if (player != null)
        {
            health = player.GetComponent<Health>();

            if (health == null)
            {
                health = player.GetComponentInParent<Health>();
            }
        }

        if (health != null)
        {
            health.TakeDamage(damage);

            Debug.Log("BK mistede " + damage + " HP!");
        }

        Destroy(gameObject);
    }
}
