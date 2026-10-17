
using UnityEngine;

public class MonkeyGrabber : MonoBehaviour
{
    [Header("Grabbing")]
    public float grabRange = 1.5f;
    public LayerMask monkeyLayer;
    public Transform holdPoint;

    private MonkeyGrabbable heldMonkey;
    private Rigidbody2D playerRb;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryGrabMonkey();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ThrowMonkey();
        }
    }

    void TryGrabMonkey()
    {
        if (heldMonkey != null)
            return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            grabRange,
            monkeyLayer
        );

        foreach (Collider2D hit in hits)
        {
            MonkeyGrabbable monkey =
                hit.GetComponentInParent<MonkeyGrabbable>();

            if (monkey != null && monkey.Grab(holdPoint))
            {
                heldMonkey = monkey;
                break;
            }
        }
    }

    void ThrowMonkey()
    {
        if (heldMonkey == null)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");

        Vector2 direction = horizontal < 0f
            ? Vector2.left
            : Vector2.right;

        // Make the monkey launch upward in an arc
        direction += Vector2.up * 0.8f;


        heldMonkey.Throw(direction);

        heldMonkey = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grabRange);
    }
}
