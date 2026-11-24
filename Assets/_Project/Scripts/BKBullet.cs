
using UnityEngine;

public class BKBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 3f;

    private Vector2 direction = Vector2.right;
    private bool hasHit = false;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        if (newDirection.sqrMagnitude > 0.001f)
        {
            direction = newDirection.normalized;

            // Vend det blå skud, så det følger BK's retning
            SpriteRenderer bulletSprite =
                GetComponent<SpriteRenderer>();

            if (bulletSprite != null)
            {
                bulletSprite.flipX = direction.x < 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (hasHit)
            return;

        transform.position +=
            (Vector3)(direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleHit(other);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit(collision.collider);
    }

    private void HandleHit(Collider2D other)
    {
        if (hasHit)
            return;

        // Ignorer BK
        if (other.GetComponentInParent<PlayerController>() != null)
            return;

        // Ram Security Guard
        SecurityGuardHealth guard =
            other.GetComponentInParent<SecurityGuardHealth>();

        if (guard != null)
        {
            hasHit = true;

            // Stop kuglen med det samme
            StopBullet();

            // Giv vagten skade
            guard.TakeDamage(damage);

            // Fjern kuglen
            Destroy(gameObject);
            return;
        }

        // Ram Donald Dump
        DumpBoss boss =
            other.GetComponentInParent<DumpBoss>();

        if (boss != null)
        {
            hasHit = true;

            StopBullet();

            boss.TakeDamage(damage);

            Destroy(gameObject);
            return;
        }

        // Ram vægge eller gulv
        if (!other.isTrigger)
        {
            hasHit = true;

            StopBullet();

            Destroy(gameObject);
        }
    }

    private void StopBullet()
    {
        // Stop bevægelse
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // Skjul kuglen straks ved træf
        SpriteRenderer bulletSprite =
            GetComponent<SpriteRenderer>();

        if (bulletSprite != null)
        {
            bulletSprite.enabled = false;
        }

        // Slå collider fra, så kuglen ikke rammer flere
        Collider2D bulletCollider =
            GetComponent<Collider2D>();

        if (bulletCollider != null)
        {
            bulletCollider.enabled = false;
        }
    }
}
