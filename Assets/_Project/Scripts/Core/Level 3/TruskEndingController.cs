
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TruskEndingController : MonoBehaviour
{
    [Header("Player")]
    public MonoBehaviour[] playerControlScripts;

    [Header("Camera")]
    public Transform mainCamera;
    public MonoBehaviour cameraFollowScript;

    [Header("Alon Escape")]
    public GameObject normalAlon;
    public GameObject escapeSpaceship;
    public float escapeSpeed = 4f;
    public float escapeDuration = 4f;

    [Header("Ending UI")]
    public GameObject escapePanel;
    public GameObject successPanel;

    [Header("Mission Failed Fade")]
    public Image fadeOverlay;
    public TMP_Text missionFailedText;
    public float fadeDuration = 2f;

    [Header("Restart")]
    public float restartDelay = 3f;

    private bool endingStarted = false;

    private void Start()
    {
        if (escapePanel != null)
            escapePanel.SetActive(false);

        if (successPanel != null)
            successPanel.SetActive(false);

        if (escapeSpaceship != null)
            escapeSpaceship.SetActive(false);

        if (fadeOverlay != null)
        {
            Color color = fadeOverlay.color;
            color.a = 0f;
            fadeOverlay.color = color;
        }

        if (missionFailedText != null)
            missionFailedText.gameObject.SetActive(false);
    }

    public void TriggerAlonEscape()
    {
        if (endingStarted)
            return;

        endingStarted = true;

        StopPlayerControls();

        StartCoroutine(AlonEscapeSequence());
    }

    private IEnumerator AlonEscapeSequence()
    {
        // Stop the camera following BK.
        if (cameraFollowScript != null)
            cameraFollowScript.enabled = false;

        if (escapeSpaceship == null)
        {
            Debug.LogError("Escape Spaceship is not assigned!");
            yield break;
        }

        // Place the escape ship at Alon's current position.
        if (normalAlon != null)
        {
            escapeSpaceship.transform.position =
                normalAlon.transform.position;
        }

        escapeSpaceship.SetActive(true);

        // Hide the original boss.
        if (normalAlon != null)
            normalAlon.SetActive(false);

        float elapsed = 0f;

        // Escape diagonally upward through space.
        Vector3 escapeDirection =
            new Vector3(0.5f, 1f, 0f).normalized;

        while (elapsed < escapeDuration)
        {
            escapeSpaceship.transform.position +=
                escapeDirection *
                escapeSpeed *
                Time.deltaTime;

            elapsed += Time.deltaTime;

            yield return null;
        }

        yield return StartCoroutine(FadeToMissionFailed());
    }

    private IEnumerator FadeToMissionFailed()
    {
        if (fadeOverlay == null)
        {
            Debug.LogError("FadeOverlay is not assigned!");
            yield break;
        }

        Color color = fadeOverlay.color;
        color.a = 0f;
        fadeOverlay.color = color;

        if (missionFailedText != null)
            missionFailedText.gameObject.SetActive(false);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            color.a = Mathf.Clamp01(
                elapsed / Mathf.Max(fadeDuration, 0.01f)
            );

            fadeOverlay.color = color;

            yield return null;
        }

        color.a = 1f;
        fadeOverlay.color = color;

        if (missionFailedText != null)
        {
            missionFailedText.text =
                "MISSION FAILED\nALON TRUSK ESCAPED!";

            missionFailedText.gameObject.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(restartDelay);

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    [Header("Mission Success")]
    public float successScreenDuration = 4f;

    public void TriggerMissionSuccess()
    {
        if (endingStarted)
            return;

        endingStarted = true;

        // Stop BK from moving.
        StopPlayerControls();

        // Display the SuccessPanel.
        if (successPanel != null)
            successPanel.SetActive(true);

        // Start the return-to-level select sequence.
        StartCoroutine(ReturnToLevelSelect());
    }

    private IEnumerator ReturnToLevelSelect()
    {
        // Keep the SuccessPanel visible for 4 seconds.
        yield return new WaitForSecondsRealtime(successScreenDuration);

        // Restore normal game speed.
        Time.timeScale = 1f;

        // Load the Intro scene.
        SceneLoader.LoadLevel("LevelSelect");
    }

    private void StopPlayerControls()
    {
        foreach (MonoBehaviour script in playerControlScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        Time.timeScale = 1f;
    }
}

