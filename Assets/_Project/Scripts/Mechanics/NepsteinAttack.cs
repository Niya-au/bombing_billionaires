using UnityEngine;

public class NepsteinAttack : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    public float shootInterval = 2f;
    public float bulletSpeed = 8f;

    private float shootTimer;

    void Update()
    {
        if (player == null || bulletPrefab == null || shootPoint == null)
            return;

        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f;
        }

        Debug.Log("NEPSTEIN SHOOTS!");
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            shootPoint.position,
            Quaternion.identity
        );

        Vector2 direction =
            (player.position - shootPoint.position).normalized;

        BossProjectile projectile =
            bullet.GetComponent<BossProjectile>();

        if (projectile != null)
        {
            projectile.SetDirection(direction, bulletSpeed);
        }
    }
}