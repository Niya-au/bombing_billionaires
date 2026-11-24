
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
        if (dumpBoss == null)
        {
            dumpBoss = FindFirstObjectByType<DumpBoss>();
        }

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

        // Skyd med F
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

        // Find BK's retning ud fra kroppens rotation via scale
        Vector2 direction = transform.localScale.x >= 0f
            ? Vector2.right
            : Vector2.left;

        // Brug FirePoint, som automatisk følger BK
        Vector3 spawnPosition = firePoint.position;

        // Opret kuglen
        GameObject bullet = Instantiate(
            bulletPrefab,
            spawnPosition,
            Quaternion.identity
        );

        BKBullet bulletScript = bullet.GetComponent<BKBullet>();

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
