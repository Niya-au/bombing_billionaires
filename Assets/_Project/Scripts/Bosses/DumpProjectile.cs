using UnityEngine;

public class DumpProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 10;

    private Vector2 direction = Vector2.left;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        if (newDirection.sqrMagnitude > 0.001f)
            direction = newDirection.normalized;
    }

    private void Update()
    {
        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("DumpBoss projectile hit the player!");

            // Connect to the player's health system later.
            // Example: playerHealth.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
