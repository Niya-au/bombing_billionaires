using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();

        if (health != null)
        {
            health.OnDied.AddListener(Die);
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnDied.RemoveListener(Die);
        }
    }
}