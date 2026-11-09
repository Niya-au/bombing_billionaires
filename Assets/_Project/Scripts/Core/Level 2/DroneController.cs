
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float moveSpeed = 2f;
    public float arrivalDistance = 0.15f;

    [Header("Shooting")]
    public Transform player;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootRange = 10f;
    public float shootInterval = 2f;
    public float bulletSpeed = 8f;

    [Header("Contact Damage")]
    public int contactDamage = 15;
    public float damageCooldown = 1f;

    private Rigidbody2D rb;
    private int currentPoint = 0;
    private float shootTimer;
    private float nextDamageTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        shootTimer = shootInterval;

        if (patrolPoints.Length > 0 &&
            patrolPoints[0] != null)
        {
            rb.position = patrolPoints[0].position;
        }
    }

    void FixedUpdate()
    {
        Patrol();
    }

    void Update()
    {
        ShootAtPlayer();
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPoint];

        if (targetPoint == null)
            return;

        Vector2 nextPosition = Vector2.MoveTowards(
            rb.position,
            targetPoint.position,
            moveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(nextPosition);

        if (Vector2.Distance(
            nextPosition,
            targetPoint.position
        ) <= arrivalDistance)
        {
            currentPoint =
                (currentPoint + 1) % patrolPoints.Length;
        }
    }

    void ShootAtPlayer()
    {
        if (player == null ||
            bulletPrefab == null ||
            shootPoint == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance > shootRange)
        {
            shootTimer = shootInterval;
            return;
        }

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            FireBullet();
            shootTimer = shootInterval;
        }
    }

    void FireBullet()
    {
        Vector2 direction = (
            player.position - shootPoint.position
        ).normalized;

        GameObject bullet = Instantiate(
            bulletPrefab,
            shootPoint.position,
            Quaternion.identity
        );

        Rigidbody2D bulletRb =
            bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity =
                direction * bulletSpeed;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Time.time < nextDamageTime)
            return;

        Health health =
            other.GetComponentInParent<Health>();

        if (health != null)
        {
            health.TakeDamage(contactDamage);

            nextDamageTime =
                Time.time + damageCooldown;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (patrolPoints == null ||
            patrolPoints.Length < 2)
            return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Transform a = patrolPoints[i];
            Transform b =
                patrolPoints[(i + 1) % patrolPoints.Length];

            if (a != null && b != null)
            {
                Gizmos.DrawLine(a.position, b.position);
                Gizmos.DrawWireSphere(a.position, 0.2f);
            }
        }
    }
}
