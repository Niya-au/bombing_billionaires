
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damage = 25;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BossBase boss =
            other.GetComponentInParent<BossBase>();

        if (boss != null)
        {
            boss.TakeDamage(damage);

            Debug.Log("Bomb hit Jaff!");

            Destroy(gameObject);
        }
    }
}
