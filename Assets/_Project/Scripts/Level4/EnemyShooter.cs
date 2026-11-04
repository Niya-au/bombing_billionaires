using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float bulletSpeed = 6f;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float shootingRange = 10f;

    private Transform player;
    private float nextShotTime;

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("EnemyShooter kunne ikke finde Player!");
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance =
            Vector2.Distance(transform.position, player.position);

        // Kan skyde både mens den går og står stille
        if (distance <= shootingRange &&
            Time.time >= nextShotTime)
        {
            Shoot();

            nextShotTime =
                Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        Vector2 direction =
            ((Vector2)player.position -
             (Vector2)firePoint.position).normalized;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        // Ignorer fjendens egen collider
        Collider2D enemyCollider =
            GetComponent<Collider2D>();

        Collider2D bulletCollider =
            bullet.GetComponent<Collider2D>();

        if (enemyCollider != null &&
            bulletCollider != null)
        {
            Physics2D.IgnoreCollision(
                bulletCollider,
                enemyCollider
            );
        }

        Rigidbody2D bulletRb =
            bullet.GetComponent<Rigidbody2D>();

        bulletRb.linearVelocity =
            direction * bulletSpeed;
    }
}