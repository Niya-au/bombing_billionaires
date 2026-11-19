using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 4f;

    [SerializeField] private SpriteRenderer visualRenderer;
    [SerializeField] private Transform firePoint;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    private float firePointDistance;
    private bool wasMoving;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Finder Animator på Enemy > Visual
        animator = GetComponentInChildren<Animator>();

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        if (firePoint != null)
        {
            firePointDistance =
                Mathf.Abs(firePoint.localPosition.x);
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        float distance =
            Mathf.Abs(player.position.x - transform.position.x);

        float direction =
            Mathf.Sign(player.position.x - transform.position.x);

        // Vend robotten
        if (visualRenderer != null)
        {
            visualRenderer.flipX = direction < 0;
        }

        // Flyt FirePoint til den side robotten vender
        if (firePoint != null)
        {
            Vector3 position = firePoint.localPosition;

            position.x =
                firePointDistance * direction;

            firePoint.localPosition = position;
        }

        bool isMoving = distance > stopDistance;

        // Bevægelse
        if (isMoving)
        {
            rb.linearVelocity =
                new Vector2(
                    direction * moveSpeed,
                    rb.linearVelocity.y
                );
        }
        else
        {
            rb.linearVelocity =
                new Vector2(
                    0f,
                    rb.linearVelocity.y
                );
        }

        // Animation
        if (animator != null)
        {
            if (isMoving)
            {
                animator.speed = 1f;
            }
            else
            {
                animator.speed = 0f;

                // Når robotten stopper,
                // vis første frame af walk-animationen
                if (wasMoving)
                {
                    animator.Play(
                        "Enemy_Walk",
                        0,
                        0f
                    );
                }
            }
        }

        wasMoving = isMoving;
    }
}