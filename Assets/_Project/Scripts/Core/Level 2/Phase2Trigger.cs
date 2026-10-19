
using UnityEngine;

public class Phase2Trigger : MonoBehaviour
{
    public GameObject phase2Enemies;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            phase2Enemies.SetActive(true);

            Debug.Log("Phase 2 started!");

            gameObject.SetActive(false);
        }
    }
}
