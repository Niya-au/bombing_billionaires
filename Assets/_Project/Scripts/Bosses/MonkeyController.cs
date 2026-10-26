
using UnityEngine;

public class MonkeyController : MonoBehaviour
{

    [Header("Phase Settings")]
    public bool canRetreat = true;

    [Header("References")]
    public Transform player;
    public GameObject nutPrefab;
    public Transform throwPoint;

    [Header("Throwing")]
    public float throwInterval = 2f;
    public float nutSpeed = 8f;
    public float attackRange = 10f;

    [Header("Retreating")]
    public Transform[] retreatPoints;
    public float retreatDistance = 3f;
    public float jumpDuration = 0.7f;
    public float jumpHeight = 2f;

    private int currentPoint = 0;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private Vector3 jumpStart;
    private Vector3 jumpEnd;
    private float throwTimer;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalThrowPointPosition;

    void Start()
    {
        throwTimer = throwInterval;

        if (canRetreat && retreatPoints.Length > 0 && retreatPoints[0] != null)
        {
            transform.position = retreatPoints[0].position;
        }

        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (throwPoint != null)
        {
            originalThrowPointPosition = throwPoint.localPosition;
        }
    }

    void Update()
    {
        if (player == null)
            return;

        // Face the next vine while jumping.
        // Otherwise, face the player.
        bool faceLeft;

        if (isJumping)
        {
            faceLeft = jumpEnd.x < jumpStart.x;
        }
        else
        {
            faceLeft = player.position.x < transform.position.x;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = faceLeft;
        }

        // Move the projectile spawn point to the correct side.
        if (throwPoint != null)
        {
            Vector3 position = originalThrowPointPosition;
            position.x = faceLeft
                ? -Mathf.Abs(originalThrowPointPosition.x)
                : Mathf.Abs(originalThrowPointPosition.x);

            throwPoint.localPosition = position;
        }

        if (isJumping)
        {
            UpdateJump();
            return;
        }

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        // Retreat to the next vine.
        if (canRetreat && distance < retreatDistance && currentPoint < retreatPoints.Length - 1)
        {
            StartJump();
            return;
        }

        // Throw nuts when the player is nearby.
        if (distance <= attackRange)
        {
            throwTimer -= Time.deltaTime;

            if (throwTimer <= 0f)
            {
                ThrowNut();
                throwTimer = throwInterval;
            }
        }
    }

    void StartJump()
    {
        if (retreatPoints[currentPoint + 1] == null)
            return;

        jumpStart = transform.position;
        jumpEnd = retreatPoints[currentPoint + 1].position;

        // Face the destination vine immediately.
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = jumpEnd.x < jumpStart.x;
        }

        isJumping = true;
        jumpTimer = 0f;

        if (animator != null)
        {
            animator.SetBool("IsJumping", true);
        }
    }

    void UpdateJump()
    {
        jumpTimer += Time.deltaTime;

        float t = Mathf.Clamp01(
            jumpTimer / Mathf.Max(jumpDuration, 0.01f)
        );

        // Move horizontally toward the next vine.
        Vector3 position = Vector3.Lerp(
            jumpStart,
            jumpEnd,
            t
        );

        // Add a curved jump arc.
        position.y += Mathf.Sin(t * Mathf.PI) * jumpHeight;

        transform.position = position;

        if (t >= 1f)
        {
            transform.position = jumpEnd;
            isJumping = false;
            if (animator != null)
                animator.SetBool("IsJumping", false);
            currentPoint++;
            throwTimer = throwInterval;
        }
    }

    void ThrowNut()
    {
        if (nutPrefab == null || throwPoint == null)
            return;

        if (animator != null)
            animator.SetTrigger("Throw");

        GameObject nut = Instantiate(
            nutPrefab,
            throwPoint.position,
            Quaternion.identity
        );

        Rigidbody2D nutRb = nut.GetComponent<Rigidbody2D>();

        if (nutRb != null)
        {
            Vector2 direction =
                (player.position - throwPoint.position).normalized;

            nutRb.linearVelocity = direction * nutSpeed;
        }
    }
}
