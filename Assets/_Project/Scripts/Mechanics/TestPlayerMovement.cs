
using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 9f;

    [Header("References")]
    public Rigidbody2D rb;
    public Transform playerVisual;
    public Animator animator;

    private bool isGrounded = false;

    void Update()
    {
        // MOVE LEFT / RIGHT
        float horizontal = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(
            horizontal * speed,
            rb.linearVelocity.y
        );

        // UPDATE WALK ANIMATION
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        // JUMP WITH UP ARROW
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            isGrounded = false;
        }

        // FLIP RIGHT
        if (horizontal > 0)
        {
            Vector3 scale = playerVisual.localScale;
            scale.x = Mathf.Abs(scale.x);
            playerVisual.localScale = scale;
        }

        // FLIP LEFT
        if (horizontal < 0)
        {
            Vector3 scale = playerVisual.localScale;
            scale.x = -Mathf.Abs(scale.x);
            playerVisual.localScale = scale;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
