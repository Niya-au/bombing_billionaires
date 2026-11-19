using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;

    private int shootDirection = 1;

    private void Update()
    {
        // Husk hvilken retning spilleren sidst gik
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            shootDirection = -1;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            shootDirection = 1;
        }

        // Skyd med F
        if (Input.GetKeyDown(KeyCode.F))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        bulletRb.linearVelocity =
            new Vector2(shootDirection * bulletSpeed, 0f);

        Destroy(bullet, 3f);
    }
}