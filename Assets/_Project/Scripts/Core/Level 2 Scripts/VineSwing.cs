
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class VineSwing : MonoBehaviour
{
    [Header("Grabbing")]
    public float grabRange = 2f;
    public LayerMask vineLayer;

    [Header("Swinging")]
    public float swingForce = 20f;
    public float releaseBoost = 1.15f;
    public float momentumTime = 0.4f;

    [Header("Climbing")]
    public float climbSpeed = 3f;
    public float minimumLength = 0.7f;

    [Header("Regrabbing")]
    public float regrabCooldown = 0.3f;

    private float regrabTimer;
    private Collider2D lastVineCollider;

    private Rigidbody2D rb;
    private DistanceJoint2D joint;
    private VineController currentVine;

    private bool attached;
    private float momentumTimer;
    private float maximumLength;

    public bool IsSwinging()
    {
        return attached || momentumTimer > 0f;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        joint = GetComponent<DistanceJoint2D>();

        if (joint == null)
            joint = gameObject.AddComponent<DistanceJoint2D>();

        joint.enabled = false;
        joint.autoConfigureDistance = false;
        joint.autoConfigureConnectedAnchor = false;
        joint.maxDistanceOnly = false;
    }


    void Update()
    {
        if (regrabTimer > 0f)
            regrabTimer -= Time.deltaTime;

        if (momentumTimer > 0f)
            momentumTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.K))
            GrabVine();

        if (Input.GetKeyUp(KeyCode.K))
            ReleaseVine();
    }

    void FixedUpdate()
    {
        if (!attached)
            return;

        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.A))
            horizontal = -1f;

        if (Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.D))
            horizontal = 1f;

        if (Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.W))
            vertical = 1f;

        if (Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.S))
            vertical = -1f;

        // Direction from the anchor to the player.
        Vector2 ropeDirection =
            rb.position - joint.connectedAnchor;

        // Perpendicular direction for swinging.
        Vector2 tangent = new Vector2(
            -ropeDirection.y,
            ropeDirection.x
        ).normalized;

        // Keep left/right controls intuitive.
        if (tangent.x < 0f)
            tangent = -tangent;

        rb.AddForce(
            tangent * horizontal * swingForce,
            ForceMode2D.Force
        );


        // Calculate the player's angle relative to the vine.
        Vector2 direction = rb.position - joint.connectedAnchor;

        float playerAngle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg + 90f;

        playerAngle = Mathf.DeltaAngle(0f, playerAngle);

        // Get the maximum angle from VineController.
        float maxAngle = currentVine.maxSwingAngle;

        // Prevent the player from moving beyond the limit.
        if (Mathf.Abs(playerAngle) > maxAngle)
        {
            float clampedAngle = Mathf.Clamp(
                playerAngle,
                -maxAngle,
                maxAngle
            );

            float radians = clampedAngle * Mathf.Deg2Rad;

            Vector2 limitedDirection = new Vector2(
                Mathf.Sin(radians),
                -Mathf.Cos(radians)
            );

            rb.position = joint.connectedAnchor +
                limitedDirection * joint.distance;


            Vector2 tangentAtLimit = new Vector2(
                Mathf.Cos(radians),
                Mathf.Sin(radians)
            );

            float tangentialSpeed = Vector2.Dot(
                rb.linearVelocity,
                tangentAtLimit
            );

            // Stop only movement pushing beyond the limit.
            if ((clampedAngle > 0f && tangentialSpeed > 0f) ||
                (clampedAngle < 0f && tangentialSpeed < 0f))
            {
                rb.linearVelocity -=
                    tangentAtLimit * tangentialSpeed;
            }

        }

        // Climb by shortening or lengthening the rope.
        float newLength = joint.distance -
            vertical * climbSpeed * Time.fixedDeltaTime;

        joint.distance = Mathf.Clamp(
            newLength,
            minimumLength,
            maximumLength
        );
    }

    void GrabVine()
    {
        if (attached)
            return;

        Collider2D vineCollider =
            Physics2D.OverlapCircle(
                transform.position,
                grabRange,
                vineLayer
            );

        if (vineCollider == null)
            return;

        // Prevent immediately regrabbing the same vine.
        if (vineCollider == lastVineCollider &&
            regrabTimer > 0f)
        {
            return;
        }

        VineController vine =
            vineCollider.GetComponentInParent<VineController>();

        if (vine == null)
        {
            Debug.LogWarning(
                "Vine script not found on parent!"
            );
            return;
        }

        currentVine = vine;

        Vector2 anchor = vine.transform.position;

        joint.connectedBody = null;
        joint.connectedAnchor = anchor;

        float distance = Vector2.Distance(
            rb.position,
            anchor
        );

        maximumLength = vine.vineLength;

        joint.distance = Mathf.Clamp(
            distance,
            minimumLength,
            maximumLength
        );

        joint.enabled = true;

        attached = true;
        momentumTimer = 0f;

        currentVine.AttachPlayer(transform);
        lastVineCollider = vineCollider;
    }

    void ReleaseVine()
    {
        if (!attached)
            return;

        joint.enabled = false;
        attached = false;

        rb.linearVelocity *= releaseBoost;

        momentumTimer = momentumTime;
        regrabTimer = regrabCooldown;

        if (currentVine != null)
        {
            currentVine.DetachPlayer();
            currentVine = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            transform.position,
            grabRange
        );
    }
}

