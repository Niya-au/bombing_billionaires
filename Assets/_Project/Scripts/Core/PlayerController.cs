using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.4f, 0.1f);
    [SerializeField] private Vector2 groundCheckOffset = new Vector2(0f, -0.5f);

    private Rigidbody2D rb;
    private VineSwing vineSwing;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vineSwing = GetComponent<VineSwing>();
    }

    void Update()
    {
        if (vineSwing != null && vineSwing.IsSwinging())
        {
            return;
        }

        Vector2 checkPos =
            (Vector2)transform.position + groundCheckOffset;

        isGrounded = Physics2D.OverlapBox(
            checkPos,
            groundCheckSize,
            0f,
            groundLayer
        );

        // LEFT
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rb.linearVelocity =
                new Vector2(-moveSpeed, rb.linearVelocity.y);
        }

        // RIGHT
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity =
                new Vector2(moveSpeed, rb.linearVelocity.y);
        }

        // Stop horizontal movement when grounded
        else if (isGrounded)
        {
            rb.linearVelocity =
                new Vector2(0, rb.linearVelocity.y);
        }

        // JUMP
        bool jumpPressed =
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.Space);

        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity =
                new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}