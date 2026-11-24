
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelEndingController : MonoBehaviour
{
    [Header("Player")]
    public MonoBehaviour[] playerControlScripts;

    [Header("Camera")]
    public Transform mainCamera;
    public MonoBehaviour cameraFollowScript;
    public Transform jaffEscapePoint;

    [Header("Jaff Escape")]
    public GameObject escapeJaffDrone;
    public GameObject normalJaff;
    public float droneFlySpeed = 3f;
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

    void Start()
    {
        if (escapePanel != null)
            escapePanel.SetActive(false);

        if (successPanel != null)
            successPanel.SetActive(false);

        if (escapeJaffDrone != null)
            escapeJaffDrone.SetActive(false);

        if (fadeOverlay != null)
        {
            Color color = fadeOverlay.color;
            color.a = 0f;
            fadeOverlay.color = color;
        }

        if (missionFailedText != null)
            missionFailedText.gameObject.SetActive(false);
    }

    public void TriggerJaffEscape()
    {
        if (endingStarted)
            return;

        endingStarted = true;
        StopPlayerControls();

        StartCoroutine(JaffEscapeSequence());
    }

    public void TriggerPlayerDeath()
    {
        if (endingStarted)
            return;

        endingStarted = true;

        Debug.Log("BK died! Starting mission failed sequence.");

        // Disable movement and other player controls.
        StopPlayerControls();

        // Reuse the existing fade-to-black and restart.
        StartCoroutine(FadeToMissionFailed());
    }

    IEnumerator JaffEscapeSequence()
    {
        // Stop normal camera following.
        if (cameraFollowScript != null)
            cameraFollowScript.enabled = false;

        // Position the drone where Jaff currently is.
        if (escapeJaffDrone != null)
        {
            if (normalJaff != null)
            {
                escapeJaffDrone.transform.position =
                    normalJaff.transform.position;
            }

            escapeJaffDrone.SetActive(true);

            Debug.Log("Escape drone activated at: " +
                      escapeJaffDrone.transform.position);
        }
        else
        {
            Debug.LogError("Escape Jaff Drone is not assigned!");
            yield break;
        }

        // Hide normal Jaff after positioning the drone.
        if (normalJaff != null)
            normalJaff.SetActive(false);

        // Position the camera on the drone.
        if (mainCamera != null)
        {
            mainCamera.position = new Vector3(
                escapeJaffDrone.transform.position.x,
                escapeJaffDrone.transform.position.y,
                mainCamera.position.z
            );
        }

        // Fly upward while the camera follows.
        float elapsed = 0f;

        while (elapsed < escapeDuration)
        {
            escapeJaffDrone.transform.position +=
                Vector3.up * droneFlySpeed * Time.deltaTime;

            if (mainCamera != null)
            {
                Vector3 targetPosition = new Vector3(
                    escapeJaffDrone.transform.position.x,
                    escapeJaffDrone.transform.position.y,
                    mainCamera.position.z
                );

                mainCamera.position = Vector3.Lerp(
                    mainCamera.position,
                    targetPosition,
                    4f * Time.deltaTime
                );
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Fade to black and show Mission Failed.
        yield return StartCoroutine(FadeToMissionFailed());
    }


    private IEnumerator FadeToMissionFailed()
    {
        if (fadeOverlay == null)
        {
            Debug.LogError("FadeOverlay is not assigned!");
            yield break;
        }

        // Start with a transparent black overlay.
        Color color = fadeOverlay.color;
        color.a = 0f;
        fadeOverlay.color = color;

        // Hide the text while the screen fades.
        if (missionFailedText != null)
            missionFailedText.gameObject.SetActive(false);

        float elapsed = 0f;

        // Gradually fade to black.
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float alpha = Mathf.Clamp01(
                elapsed / Mathf.Max(fadeDuration, 0.01f)
            );

            color.a = alpha;
            fadeOverlay.color = color;

            yield return null;
        }

        // Make sure the screen is completely black.
        color.a = 1f;
        fadeOverlay.color = color;

        // Reveal the mission failed message.
        if (missionFailedText != null)
            missionFailedText.gameObject.SetActive(true);

        Debug.Log("MISSION FAILED");

        // Let the player read the failure message.
        yield return new WaitForSecondsRealtime(restartDelay);

        // Resume normal game time.
        Time.timeScale = 1f;

        // Reload Level 2.
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


    void StopPlayerControls()
    {
        foreach (MonoBehaviour script in playerControlScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        // Keep time running for the escape animation.
        Time.timeScale = 1f;
    }
}
