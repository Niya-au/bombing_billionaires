using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 9f;

    public Rigidbody2D rb;
    public Transform playerVisual;

    private bool isGrounded = false;

    void Update()
    {
        // GÅ VENSTRE / HØJRE
        float horizontal = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(
            horizontal * speed,
            rb.linearVelocity.y
        );

        // HOP MED UP ARROW
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            isGrounded = false;
        }

        // FLIP HØJRE
        if (horizontal > 0)
        {
            Vector3 scale = playerVisual.localScale;
            scale.x = Mathf.Abs(scale.x);
            playerVisual.localScale = scale;
        }

        // FLIP VENSTRE
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