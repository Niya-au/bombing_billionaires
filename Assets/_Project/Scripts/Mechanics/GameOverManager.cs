
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("Fade UI")]
    public Image fadeOverlay;
    public TMP_Text missionFailedText;

    [Header("Player Controls")]
    public MonoBehaviour[] playerControlScripts;

    [Header("Timing")]
    public float fadeDuration = 2f;
    public float restartDelay = 3f;

    private bool gameOverStarted = false;

    private void Start()
    {
        // Make the overlay transparent initially.
        if (fadeOverlay != null)
        {
            Color color = fadeOverlay.color;
            color.a = 0f;
            fadeOverlay.color = color;
        }

        // Hide the Mission Failed text.
        if (missionFailedText != null)
        {
            missionFailedText.gameObject.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (gameOverStarted)
            return;

        gameOverStarted = true;

        Debug.Log("BK died! Starting Game Over.");

        // Disable BK's movement and input scripts.
        foreach (MonoBehaviour script in playerControlScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        if (fadeOverlay == null)
        {
            Debug.LogError("Fade Overlay is not assigned!");
            yield break;
        }

        // Keep time running during the fade.
        Time.timeScale = 1f;

        Color color = fadeOverlay.color;
        color.a = 0f;
        fadeOverlay.color = color;

        float elapsed = 0f;

        // Fade to black.
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            color.a = Mathf.Clamp01(
                elapsed / Mathf.Max(fadeDuration, 0.01f)
            );

            fadeOverlay.color = color;

            yield return null;
        }

        // Fully black screen.
        color.a = 1f;
        fadeOverlay.color = color;

        // Display Mission Failed.
        if (missionFailedText != null)
        {
            missionFailedText.gameObject.SetActive(true);
        }

        Debug.Log("MISSION FAILED");

        // Wait before restarting.
        yield return new WaitForSecondsRealtime(restartDelay);

        Retry();
    }

    public void Retry()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
