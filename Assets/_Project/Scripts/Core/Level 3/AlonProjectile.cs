using UnityEngine;

public class AlonProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Health health =
            other.GetComponent<Health>();

        if (health != null)
        {
            // Bruger din eksisterende Health damage-funktion
            other.gameObject.SendMessage(
                "TakeDamage",
                damage,
                SendMessageOptions.DontRequireReceiver
            );

            Debug.Log("ALON PROJECTILE HIT BK!");

            Destroy(gameObject);
            return;
        }

        // Hvis den rammer noget andet
        if (!other.CompareTag("Hazard"))
        {
            Destroy(gameObject);
        }
    }
}