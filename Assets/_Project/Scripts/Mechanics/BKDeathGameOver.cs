using UnityEngine;

public class BKDeathGameOver : MonoBehaviour
{
    public Health playerHealth;
    public GameOverManager gameOverManager;

    private bool gameOverTriggered = false;

    void Update()
    {
        if (gameOverTriggered) return;

        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            gameOverTriggered = true;

            if (gameOverManager != null)
            {
                gameOverManager.GameOver();
            }
        }
    }
}