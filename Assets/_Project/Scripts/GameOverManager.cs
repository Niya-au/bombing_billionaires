
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Victory")]
    [SerializeField] private GameObject victoryPanel;

    private bool gameFinished;

    private void Start()
    {
        Time.timeScale = 1f;
        gameFinished = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ShowVictory()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
