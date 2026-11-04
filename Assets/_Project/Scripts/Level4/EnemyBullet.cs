using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Hvis kuglen rammer Player
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth =
                collision.gameObject.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        // Kuglen forsvinder efter den rammer noget
        Destroy(gameObject);
    }
}