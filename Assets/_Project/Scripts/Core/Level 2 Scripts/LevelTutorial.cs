
using System.Collections;
using UnityEngine;

public class LevelTutorial : MonoBehaviour
{
    [Header("Tutorial UI")]
    public GameObject tutorialPanel;

    [Header("Player Controls")]
    public MonoBehaviour[] playerControlScripts;

    private bool gameStarted = false;
    private bool canStart = false;

    void Start()
    {
        // Hide the tutorial while the cutscene plays.
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    void Update()
    {
        if (gameStarted || !canStart)
            return;

        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StartLevel();
        }
    }

    public void ShowTutorial()
    {
        if (gameStarted)
            return;

        // Show the tutorial after the cutscene.
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);

        // Pause gameplay.
        Time.timeScale = 0f;

        // Prevent the same Enter press from
        // skipping both the cutscene and tutorial.
        canStart = false;
        StartCoroutine(AllowStartNextFrame());

        Debug.Log("Tutorial shown!");
    }

    private IEnumerator AllowStartNextFrame()
    {
        yield return null;
        canStart = true;
    }

    public void StartLevel()
    {
        if (gameStarted || !canStart)
            return;

        gameStarted = true;
        canStart = false;

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        foreach (MonoBehaviour script in playerControlScripts)
        {
            if (script != null)
                script.enabled = true;
        }

        Time.timeScale = 1f;

        Debug.Log("Gameplay started!");
    }
}

