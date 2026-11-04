
using UnityEngine;

public class BKBullet : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 3f;

    private Vector2 direction = Vector2.right;
    private bool hasHit;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        if (newDirection.sqrMagnitude > 0.001f)
            direction = newDirection.normalized;
    }

    private void FixedUpdate()
    {
        transform.position +=
            (Vector3)(direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        // Ignore BK
        if (other.GetComponentInParent<PlayerController>() != null)
            return;

        // Hit Security Guard
        SecurityGuardHealth guard =
            other.GetComponentInParent<SecurityGuardHealth>();

        if (guard != null)
        {
            hasHit = true;
            guard.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Hit Donald Dump
        DumpBoss boss = other.GetComponentInParent<DumpBoss>();

        if (boss != null)
        {
            hasHit = true;
            boss.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Hit walls or ground
        if (!other.isTrigger)
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }
}
