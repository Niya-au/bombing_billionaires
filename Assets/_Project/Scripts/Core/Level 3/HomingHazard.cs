
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingHazard : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private bool continuouslyFollowPlayer = false;
    [SerializeField] private float turnSpeed = 3f;

    [Header("Lifetime")]
    [SerializeField] private float lifetime = 8f;

    [Header("Rotation")]
    [SerializeField] private bool faceMovementDirection = false;
    [SerializeField] private float rotationOffset = -90f;

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;

            moveDirection =
                ((Vector2)player.position - rb.position).normalized;
        }
        else
        {
            Debug.LogWarning("HomingHazard: Player not found!");
            moveDirection = Vector2.down;
        }

        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        if (continuouslyFollowPlayer && player != null)
        {
            Vector2 targetDirection =
                ((Vector2)player.position - rb.position).normalized;

            moveDirection = Vector2.Lerp(
                moveDirection,
                targetDirection,
                turnSpeed * Time.fixedDeltaTime
            ).normalized;
        }

        rb.MovePosition(
            rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime
        );

        if (faceMovementDirection && moveDirection.sqrMagnitude > 0.001f)
        {
            float angle =
                Mathf.Atan2(moveDirection.y, moveDirection.x) *
                Mathf.Rad2Deg;

            rb.MoveRotation(angle + rotationOffset);
        }
    }
}

