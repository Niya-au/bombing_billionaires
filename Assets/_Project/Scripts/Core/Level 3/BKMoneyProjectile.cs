using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BKMoneyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 5f;

    private void Start() => Destroy(gameObject, lifetime);

    private void Update() => transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);

    private void OnTriggerEnter2D(Collider2D other)
    {
        BossBase boss = other.GetComponentInParent<BossBase>();
        if (boss == null) return;
        boss.TakeDamage(damage);
        Destroy(gameObject);
    }
}
