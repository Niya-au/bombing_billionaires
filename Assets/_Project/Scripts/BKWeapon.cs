
using UnityEngine;

public class BKWeapon : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.3f;

    [Header("Boss")]
    [SerializeField] private DumpBoss dumpBoss;

    [Header("BK Visual")]
    [SerializeField] private SpriteRenderer bkSprite;

    private float nextShootTime;

    private void Start()
    {
        // Find Donald Dump automatisk
        if (dumpBoss == null)
        {
            dumpBoss = FindFirstObjectByType<DumpBoss>();
        }

        // Find BK's SpriteRenderer automatisk
        if (bkSprite == null)
        {
            Transform visual = transform.Find("TyroneVisual");

            if (visual != null)
            {
                bkSprite = visual.GetComponent<SpriteRenderer>();
            }
        }
    }

    private void Update()
    {
        // BK må ikke skyde under dialog eller gåder
        if (dumpBoss != null && dumpBoss.IsInteractionActive)
        {
            return;
        }

        // BK må skyde igen, når dialogen eller gåden er slut
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
        {
            Debug.LogWarning(
                "BKWeapon: Bullet Prefab eller Fire Point mangler!"
            );
            return;
        }

        // Find den retning, BK kigger
        Vector2 direction =
            (bkSprite != null && bkSprite.flipX)
            ? Vector2.left
            : Vector2.right;

        // Placér kuglen foran BK
        Vector3 spawnPosition = firePoint.position;

        float distance = Mathf.Abs(firePoint.localPosition.x);

        spawnPosition.x = transform.position.x +
            (direction.x > 0f ? distance : -distance);

        // Opret den blå kugle
        GameObject bullet = Instantiate(
            bulletPrefab,
            spawnPosition,
            Quaternion.identity
        );

        // Send kuglen i den rigtige retning
        BKBullet bulletScript =
            bullet.GetComponent<BKBullet>();

        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }
        else
        {
            Debug.LogWarning(
                "BKWeapon: BKBullet-script mangler på kuglen!"
            );
        }
    }
}
