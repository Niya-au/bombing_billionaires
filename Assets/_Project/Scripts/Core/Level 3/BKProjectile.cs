using UnityEngine;

public class BKProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Rammer Alon
        AlonThruskBoss boss =
            other.GetComponentInParent<AlonThruskBoss>();

        if (boss != null)
        {
            boss.TakeDamage(damage);

            Debug.Log("🔵 BK HIT ALON! -" + damage + " HP");

            Destroy(gameObject);
            return;
        }
    }
}