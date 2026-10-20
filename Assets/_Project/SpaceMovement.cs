using UnityEngine;

public class SpaceMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float thrust = 5f;

    [Header("Zero-G")]
    [SerializeField] private float gravityScale = 0f;
    [SerializeField] private float linearDamping = 2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = gravityScale;
        rb.linearDamping = linearDamping;
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);

        rb.AddForce(movement * thrust);
    }
}