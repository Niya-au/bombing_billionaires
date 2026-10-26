using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    private void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by hazard!");
            Destroy(gameObject);
        }
    }
}