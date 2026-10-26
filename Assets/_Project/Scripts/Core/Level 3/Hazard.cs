using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float moveSpeed = 4f;

    private void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);

            Debug.Log("💥 ASTEROID HIT BK! -" + damage + " HP");

            Destroy(gameObject);
        }
    }
}