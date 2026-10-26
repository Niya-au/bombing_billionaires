
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 25;

    [Header("Timing")]
    public float lifetime = 5f;
    public float explosionAnimationDuration = 1.2f;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody2D rb;
    private Collider2D bombCollider;
    private bool hasExploded = false;
    private float timer;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        rb = GetComponent<Rigidbody2D>();
        bombCollider = GetComponent<Collider2D>();

        timer = lifetime;
    }

    void Update()
    {
        if (hasExploded)
            return;

        timer -= Time.deltaTime;

        // Explode when the lifetime runs out.
        if (timer <= 0f)
        {
            Explode();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasExploded)
            return;

        BossBase boss =
            other.GetComponentInParent<BossBase>();

        if (boss != null)
        {
            Debug.Log("Bomb hit Jaff! Damage: " + damage);

            // Deal damage only once.
            boss.TakeDamage(damage);

            // Play explosion animation.
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded)
            return;

        hasExploded = true;

        // Stop the bomb moving.
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        // Disable collisions during the explosion.
        if (bombCollider != null)
        {
            bombCollider.enabled = false;
        }

        // Play the explosion animation.
        if (animator != null)
        {
            animator.SetTrigger("Explode");
        }

        Debug.Log("Bomb exploded!");

        // Wait until the animation finishes.
        Destroy(gameObject, explosionAnimationDuration);
    }
}
