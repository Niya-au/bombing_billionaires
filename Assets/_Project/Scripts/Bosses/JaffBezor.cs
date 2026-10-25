
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

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float deathAnimationDuration = 1.5f;

    [Header("Facing")]
    // Enable this if the original sprite faces left.
    public bool spriteFacesLeft = true;

    private int currentPoint = 0;
    private bool isJumping = false;
    private bool battleEnded = false;

    private Vector3 jumpStart;
    private Vector3 jumpEnd;
    private float jumpTimer;
    private float shootTimer;

    private Vector3 originalShootPointPosition;

    protected override void Start()
    {
        base.Start();

        // Find visual components on JaffVisual.
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer =
                GetComponentInChildren<SpriteRenderer>();
        }

        shootTimer = shootInterval;

        // Remember the original bullet spawn position.
        if (shootPoint != null)
        {
            originalShootPointPosition =
                shootPoint.localPosition;
        }

        // Start at the first retreat point.
        if (retreatPoints != null &&
            retreatPoints.Length > 0 &&
            retreatPoints[0] != null)
        {
            transform.position =
                retreatPoints[0].position;
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

        // Face player when idle/shooting.
        // Face destination vine while jumping.
        FaceDirection();

        if (!battleEnded)
        {
            ShootAtPlayer();
        }
    }

    void FaceDirection()
    {
        if (spriteRenderer == null)
            return;

        bool faceLeft;

        if (isJumping)
        {
            // Face the next vine.
            faceLeft = jumpEnd.x < jumpStart.x;
        }
        else
        {
            // Face the player.
            if (player == null)
                return;

            faceLeft =
                player.position.x < transform.position.x;
        }

        // Flip only the artwork, not the whole boss.
        spriteRenderer.flipX =
            spriteFacesLeft ? !faceLeft : faceLeft;

        // Move ShootPoint to the side Jaff faces.
        // ShootPoint should be a direct child of JaffBezor.
        if (shootPoint != null &&
            shootPoint.parent == transform)
        {
            Vector3 position =
                originalShootPointPosition;

            position.x = faceLeft
                ? -Mathf.Abs(originalShootPointPosition.x)
                : Mathf.Abs(originalShootPointPosition.x);

            shootPoint.localPosition = position;
        }
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
        if (retreatPoints == null ||
            currentPoint + 1 >= retreatPoints.Length ||
            retreatPoints[currentPoint + 1] == null)
            return;

        jumpStart = transform.position;

        jumpEnd =
            retreatPoints[currentPoint + 1].position;

        jumpTimer = 0f;
        isJumping = true;

        if (animator != null)
        {
            animator.SetBool("IsJumping", true);
        }

        // Turn toward the next vine immediately.
        FaceDirection();
    }

    void UpdateJump()
    {
        jumpTimer += Time.deltaTime;

        float t = Mathf.Clamp01(
            jumpTimer / Mathf.Max(jumpDuration, 0.01f)
        );

        Vector3 position = Vector3.Lerp(
            jumpStart,
            jumpEnd,
            t
        );

        // Curved jump arc.
        position.y +=
            Mathf.Sin(t * Mathf.PI) * jumpHeight;

        transform.position = position;

        if (t >= 1f)
        {
            transform.position = jumpEnd;

            currentPoint++;
            isJumping = false;

            if (animator != null)
            {
                animator.SetBool("IsJumping", false);
            }

            // Escape at the final retreat point.
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

        // Play shooting animation.
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }

        // Aim toward the player.
        Vector2 direction = (
            player.position - shootPoint.position
        ).normalized;

        // Rotate the bullet to face its travel direction.
        float angle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg;

        GameObject projectile = Instantiate(
            projectilePrefab,
            shootPoint.position,
            Quaternion.Euler(0f, 0f, angle)
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

        // Later: trigger Game Over screen.
    }

    public override void TakeDamage(int amount)
    {
        if (battleEnded)
            return;

        if (amount > 0 && animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        base.TakeDamage(amount);
    }

    protected override void OnDefeated()
    {
        if (battleEnded)
            return;

        battleEnded = true;

        base.OnDefeated();

        Die();
    }

    protected override void Die()
    {
        Debug.Log("Jaff defeated! Player wins!");

        if (animator != null)
        {
            animator.SetBool("IsJumping", false);

            animator.ResetTrigger("Shoot");
            animator.ResetTrigger("Hurt");

            animator.SetTrigger("Die");
        }

        // Allow the death animation to finish.
        Destroy(gameObject, deathAnimationDuration);
    }
}
