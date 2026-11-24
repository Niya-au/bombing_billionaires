
using UnityEngine;

public class SecurityGuardAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float stopDistance = 3f;
    [SerializeField] private float detectionRange = 7f;

    [Header("Patrol")]
    [SerializeField] private float patrolDistance = 2f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootRange = 5f;
    [SerializeField] private float shootCooldown = 1.5f;
    [SerializeField] private float bulletSpeed = 7f;

    [Header("Aim")]
    [SerializeField] private float aimHeight = 0.8f;
    [SerializeField] private float bulletSpawnOffset = 0.35f;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer guardSprite;

    private Transform player;
    private Rigidbody2D rb;
    private SecurityGuardHealth guardHealth;

    private Vector2 startPosition;
    private int patrolDirection = 1;
    private float nextShootTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        guardHealth = GetComponent<SecurityGuardHealth>();

        if (rb != null)
            startPosition = rb.position;
        else
            startPosition = transform.position;

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        if (guardSprite == null)
        {
            Transform visual = transform.Find("Guardvisual");

            if (visual != null)
                guardSprite = visual.GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance <= shootRange &&
            Time.time >= nextShootTime)
        {
            Shoot();
            nextShootTime = Time.time + shootCooldown;
        }
    }

    private void FixedUpdate()
    {
        if (player == null || rb == null)
            return;

        float difference =
            player.position.x - transform.position.x;

        float distance = Mathf.Abs(difference);

        if (distance <= detectionRange)
        {
            // Kig mod BK
            FaceDirection(difference);

            // Gå mod BK, men hold afstand
            if (distance > stopDistance)
            {
                float direction = Mathf.Sign(difference);

                rb.MovePosition(
                    rb.position +
                    new Vector2(
                        direction * moveSpeed * Time.fixedDeltaTime,
                        0f
                    )
                );
            }
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        float distanceFromStart =
            rb.position.x - startPosition.x;

        if (distanceFromStart >= patrolDistance)
            patrolDirection = -1;
        else if (distanceFromStart <= -patrolDistance)
            patrolDirection = 1;

        FaceDirection(patrolDirection);

        rb.MovePosition(
            rb.position +
            new Vector2(
                patrolDirection * moveSpeed * Time.fixedDeltaTime,
                0f
            )
        );
    }

    private void FaceDirection(float direction)
    {
        if (guardSprite != null)
            guardSprite.flipX = direction < 0f;
    }

    private void Shoot()
    {
        if (bulletPrefab == null || player == null)
            return;

        // Find ud af, om BK er til højre eller venstre
        float facingDirection =
            player.position.x >= transform.position.x
                ? 1f
                : -1f;

        // Find positionen, hvor kuglen starter
        Vector3 spawnPosition;

        if (firePoint != null)
        {
            spawnPosition = firePoint.position;
        }
        else
        {
            spawnPosition = transform.position +
                new Vector3(
                    facingDirection * 0.6f,
                    0.2f,
                    0f
                );
        }

        // Sørg for, at skuddet starter foran vagten
        if (guardSprite != null)
        {
            spawnPosition.x =
                guardSprite.bounds.center.x +
                facingDirection * bulletSpawnOffset;
        }

        // Sigt mod BK's overkrop i stedet for benene
        Vector2 targetPosition =
            (Vector2)player.position +
            new Vector2(0f, aimHeight);

        // Beregn retning fra kuglens startposition
        Vector2 direction =
            (targetPosition - (Vector2)spawnPosition).normalized;

        // Opret den røde kugle
        GameObject bullet = Instantiate(
            bulletPrefab,
            spawnPosition,
            Quaternion.identity
        );

        // Få kuglen til at flyve mod BK
        Rigidbody2D bulletRb =
            bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bulletRb.gravityScale = 0f;
            bulletRb.linearVelocity =
                direction * bulletSpeed;
        }

        // Vend den røde sprite i skyderetningen
        GuardBullet guardBullet =
            bullet.GetComponent<GuardBullet>();

        if (guardBullet != null)
        {
            guardBullet.SetDirection(direction);
        }
    }
}
