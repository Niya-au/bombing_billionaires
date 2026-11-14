using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AlonRocket : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 7f;

    private void Start() => Destroy(gameObject, lifetime);

    private void Update() => transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

    private void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.GetComponentInParent<Health>();
        if (health == null) return;
        health.TakeDamage(damage);
        Destroy(gameObject);
    }
}
