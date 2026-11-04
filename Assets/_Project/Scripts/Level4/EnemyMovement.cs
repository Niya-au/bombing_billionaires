using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 4f;

    private Transform player;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        float distance =
            Mathf.Abs(player.position.x - transform.position.x);

        if (distance > stopDistance)
        {
            float direction =
                Mathf.Sign(player.position.x - transform.position.x);

            rb.linearVelocity =
                new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity =
                new Vector2(0f, rb.linearVelocity.y);
        }
    }
}