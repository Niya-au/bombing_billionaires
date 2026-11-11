using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 4f;

    [SerializeField] private SpriteRenderer visualRenderer;
    [SerializeField] private Transform firePoint;

    private Transform player;
    private Rigidbody2D rb;

    private float firePointDistance;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Gem hvor langt FirePoint er fra midten
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

        // Flyt FirePoint til den rigtige side
        if (firePoint != null)
        {
            Vector3 position = firePoint.localPosition;

            position.x =
                firePointDistance * direction;

            firePoint.localPosition = position;
        }

        // Bevæg robotten
        if (distance > stopDistance)
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
    }
}