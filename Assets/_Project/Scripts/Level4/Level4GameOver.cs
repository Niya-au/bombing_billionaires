using UnityEngine;

public class Level4GameOver : MonoBehaviour
{
    [SerializeField] private Health tyroneHealth;
    [SerializeField] private GameObject gameOverUI;

    private void Start()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (tyroneHealth != null)
            tyroneHealth.OnDied.AddListener(ShowGameOver);
    }

    private void OnDestroy()
    {
        if (tyroneHealth != null)
            tyroneHealth.OnDied.RemoveListener(ShowGameOver);
    }

    private void ShowGameOver()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }
}