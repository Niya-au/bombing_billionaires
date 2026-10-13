
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
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

    void Start()
    {
        throwTimer = throwInterval;

        if (retreatPoints.Length > 0 &&
            retreatPoints[0] != null)
        {
            transform.position = retreatPoints[0].position;
        }
    }

    void Update()
    {
        if (player == null)
            return;

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
        if (distance < retreatDistance &&
            currentPoint < retreatPoints.Length - 1)
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

        isJumping = true;
        jumpTimer = 0f;

        jumpStart = transform.position;
        jumpEnd = retreatPoints[currentPoint + 1].position;
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
            currentPoint++;
            throwTimer = throwInterval;
        }
    }

    void ThrowNut()
    {
        if (nutPrefab == null || throwPoint == null)
            return;

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
