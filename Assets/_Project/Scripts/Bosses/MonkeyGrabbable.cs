
using UnityEngine;

public class MonkeyGrabbable : MonoBehaviour
{
    [Header("Grabbing")]
    public bool canBeGrabbed = false;
    public float throwForce = 12f;

    private bool isHeld = false;
    private Rigidbody2D rb;
    private Collider2D monkeyCollider;
    private MonkeyController monkeyController;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        monkeyCollider = GetComponent<Collider2D>();
        monkeyController = GetComponent<MonkeyController>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    public bool Grab(Transform holdPoint)
    {
        if (!canBeGrabbed || isHeld)
            return false;

        isHeld = true;

        if (monkeyController != null)
            monkeyController.enabled = false;

        if (monkeyCollider != null)
            monkeyCollider.enabled = false;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;

        return true;
    }

    public void Throw(Vector2 direction)
    {
        if (!isHeld)
            return;

        isHeld = false;

        transform.SetParent(null);

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;

        if (monkeyCollider != null)
            monkeyCollider.enabled = true;

        rb.linearVelocity = direction.normalized * throwForce;

        // Remove monkey after it flies away.
        Destroy(gameObject, 10f);
    }
}
