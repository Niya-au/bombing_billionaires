
using UnityEngine;

public class JaffBezor : BossBase
{
    [Header("Retreat")]
    public Transform[] retreatPoints;
    public float retreatDistance = 4f;
    public float jumpDuration = 0.8f;
    public float jumpHeight = 2f;

    [Header("Shooting")]
    public Transform player;
    public Transform shootPoint;
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public float projectileSpeed = 8f;
    public float shootRange = 15f;

    private int currentPoint = 0;
    private bool isJumping = false;
    private bool battleEnded = false;

    private Vector3 jumpStart;
    private Vector3 jumpEnd;
    private float jumpTimer;
    private float shootTimer;

    protected override void Start()
    {
        base.Start();

        shootTimer = shootInterval;

        if (retreatPoints.Length > 0 &&
            retreatPoints[0] != null)
        {
            transform.position = retreatPoints[0].position;
        }
    }

    void Update()
    {
        if (battleEnded)
            return;

        if (isJumping)
        {
            UpdateJump();
        }
        else
        {
            CheckRetreat();
        }

        ShootAtPlayer();
    }

    void CheckRetreat()
    {
        if (player == null ||
            retreatPoints == null ||
            retreatPoints.Length == 0)
            return;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance > retreatDistance)
            return;

        if (currentPoint < retreatPoints.Length - 1)
        {
            StartJump();
        }
        else
        {
            Escape();
        }
    }

    void StartJump()
    {
        if (retreatPoints[currentPoint + 1] == null)
            return;

        jumpStart = transform.position;
        jumpEnd = retreatPoints[currentPoint + 1].position;

        jumpTimer = 0f;
        isJumping = true;
    }

    void UpdateJump()
    {
        jumpTimer += Time.deltaTime;

        float t = Mathf.Clamp01(
            jumpTimer / jumpDuration
        );

        Vector3 position = Vector3.Lerp(
            jumpStart,
            jumpEnd,
            t
        );

        position.y += Mathf.Sin(t * Mathf.PI) * jumpHeight;

        transform.position = position;

        if (t >= 1f)
        {
            transform.position = jumpEnd;

            currentPoint++;
            isJumping = false;

            // Reaching the last point means Jaff escapes.
            if (currentPoint == retreatPoints.Length - 1)
            {
                Escape();
            }
        }
    }

    void ShootAtPlayer()
    {
        if (player == null ||
            shootPoint == null ||
            projectilePrefab == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance > shootRange)
            return;

        shootTimer -= Time.deltaTime;

        if (shootTimer > 0f)
            return;

        Vector2 direction = (
            player.position - shootPoint.position
        ).normalized;

        GameObject projectile = Instantiate(
            projectilePrefab,
            shootPoint.position,
            Quaternion.identity
        );

        Rigidbody2D rb =
            projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity =
                direction * projectileSpeed;
        }

        shootTimer = shootInterval;
    }

    void Escape()
    {
        if (battleEnded)
            return;

        battleEnded = true;

        Debug.Log("Jaff escaped! Player loses.");

        // Later: trigger your Game Over screen.
    }

    public override void TakeDamage(int amount)
    {
        if (battleEnded)
            return;

        base.TakeDamage(amount);
    }

    protected override void OnDefeated()
    {
        battleEnded = true;

        base.OnDefeated();

        // BossBase currently doesn't call Die().
        Die();
    }

    protected override void Die()
    {
        Debug.Log("Jaff defeated! Player wins!");

        Destroy(gameObject);
    }
}
