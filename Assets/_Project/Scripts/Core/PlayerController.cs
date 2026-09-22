using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.4f, 0.1f);
    [SerializeField] private Vector2 groundCheckOffset = new Vector2(0f, -0.5f);

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        // Get the Rigidbody2D attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 checkPos = (Vector2)transform.position + groundCheckOffset;
        isGrounded = Physics2D.OverlapBox(checkPos, groundCheckSize, 0f, groundLayer);

        // LEFT
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        }

        // RIGHT
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        }

        // Stop horizontal movement when neither is pressed
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        // UP / JUMP
        bool jumpPressed = (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space));
        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}