
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class FallDeathZone : MonoBehaviour
{
    private bool restarting = false;

    void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only restart when the player touches the zone.
        if (!other.CompareTag("Player") &&
            !other.transform.root.CompareTag("Player"))
        {
            return;
        }

        if (restarting)
            return;

        restarting = true;

        Debug.Log("Player fell! Restarting Level 2...");

        // Restore normal game speed before reloading.
        Time.timeScale = 1f;

        // Reload the current level.
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
