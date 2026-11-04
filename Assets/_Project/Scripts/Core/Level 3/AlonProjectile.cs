using UnityEngine;

public class AlonProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 5f;


    private void Start()
    {
        Destroy(gameObject, lifetime);

        Debug.Log("🔴 RED PROJECTILE CREATED");
    }


    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Må ikke ramme Alon selv
        if (
            other.GetComponentInParent<AlonThruskBoss>() != null
        )
        {
            return;
        }


        // Rammer BK
        Health health =
            other.GetComponentInParent<Health>();


        if (health != null)
        {
            health.TakeDamage(damage);

            Debug.Log(
                "🔴 ALON PROJECTILE HIT BK! -" +
                damage +
                " HP"
            );

            Destroy(gameObject);

            return;
        }


        // Rammer hazard
        if (other.CompareTag("Hazard"))
        {
            Destroy(gameObject);
        }
    }
}