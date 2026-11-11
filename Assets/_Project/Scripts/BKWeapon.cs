
using UnityEngine;

public class BKWeapon : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.3f;

    private float nextShootTime;

    private void Update()
    {        
    if (Input.GetKeyDown(KeyCode.F) &&
    Time.time >= nextShootTime)
        
        {
            Shoot();
            nextShootTime = Time.time + shootCooldown;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
            return;

        Vector2 direction = transform.localScale.x >= 0
            ? Vector2.right
            : Vector2.left;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        BKBullet bulletScript = bullet.GetComponent<BKBullet>();

        if (bulletScript != null)
            bulletScript.SetDirection(direction);
    }
}
