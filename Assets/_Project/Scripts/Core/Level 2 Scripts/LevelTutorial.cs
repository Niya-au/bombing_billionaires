
using UnityEngine;

public class LevelTutorial : MonoBehaviour
{
    [Header("Tutorial UI")]
    public GameObject tutorialPanel;

    private bool gameStarted = false;

    void Start()
    {
        // Show the tutorial when the level loads.
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);

        // Pause the game until the player starts.
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (gameStarted)
            return;

        // Allow Enter as an alternative to clicking Start.
        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StartLevel();
        }
    }

    public void StartLevel()
    {
        if (gameStarted)
            return;

        gameStarted = true;

        // Resume gameplay.
        Time.timeScale = 1f;

        // Hide the tutorial.
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        Debug.Log("Level 2 started!");
    }

    void OnDisable()
    {
        // Avoid leaving the game paused.
        Time.timeScale = 1f;
    }
}
