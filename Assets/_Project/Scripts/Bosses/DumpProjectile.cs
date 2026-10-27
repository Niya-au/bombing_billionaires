
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
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        if (newDirection.sqrMagnitude > 0.001f)
            direction = newDirection.normalized;
    }

    private void Update()
    {
        if (hasHit)
            return;

        if (player != null)
        {
            direction = (player.position - transform.position).normalized;
        }

        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);

        if (player != null &&
            Vector2.Distance(transform.position, player.position) <= hitDistance)
        {
            HitPlayer(player.GetComponent<Health>());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HitPlayer(other.GetComponent<Health>());
        }
    }

    private void HitPlayer(Health health)
    {
        if (hasHit)
            return;

        hasHit = true;

        if (health == null && player != null)
            health = player.GetComponentInParent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);
            Debug.Log("Tyrone lost " + damage + " HP!");
        }

        Destroy(gameObject);
    }
}
