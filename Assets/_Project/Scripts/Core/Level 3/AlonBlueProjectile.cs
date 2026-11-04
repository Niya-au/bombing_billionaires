using UnityEngine;

public class AlonBlueProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);

        Debug.Log("🔵 BLUE PROJECTILE CREATED");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Rammer Alon
        AlonThruskBoss boss =
            other.GetComponentInParent<AlonThruskBoss>();


        if (boss != null)
        {
            boss.TakeDamage(damage);

            Debug.Log(
                "🔵 BK PROJECTILE HIT ALON! -" +
                damage +
                " HP"
            );

            Destroy(gameObject);

            return;
        }


        // Rammer ikke BK med sit eget projectile
        if (other.GetComponentInParent<PlayerController>() != null)
        {
            return;
        }


        // Rammer noget andet
        if (!other.CompareTag("Hazard"))
        {
            Destroy(gameObject);
        }
    }
}