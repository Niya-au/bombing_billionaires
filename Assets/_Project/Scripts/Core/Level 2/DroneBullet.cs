
using UnityEngine;

public class DroneBullet : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore the drone and other enemies.
        if (other.GetComponentInParent<DroneController>() != null)
            return;

        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponentInParent<Health>();

            if (health != null)
                health.TakeDamage(damage);

            Destroy(gameObject);
        }
        else if (other.gameObject.layer ==
                 LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
