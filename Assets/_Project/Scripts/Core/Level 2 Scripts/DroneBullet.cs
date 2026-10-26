
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
        // Ignore the boss and drones.
        if (other.GetComponentInParent<BossBase>() != null ||
            other.GetComponentInParent<DroneController>() != null)
        {
            return;
        }

        // Look for Health on the object we hit or its parent.
        Health health = other.GetComponentInParent<Health>();

        if (health != null && health.CompareTag("Player"))
        {
            health.TakeDamage(damage);

            Debug.Log(
                "Drone bullet hit player! HP: " +
                health.currentHP
            );

            Destroy(gameObject);
            return;
        }

        // Destroy bullets that hit the ground.
        if (other.gameObject.layer ==
            LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
