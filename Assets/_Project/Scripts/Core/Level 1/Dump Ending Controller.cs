
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DumpEndingController : MonoBehaviour
{
    [Header("Player")]
    public MonoBehaviour[] playerControlScripts;
    public Rigidbody2D playerRb;

    [Header("Ending UI")]
    public GameObject successPanel;

    [Header("Mission Failed Fade")]
    public Image fadeOverlay;
    public TMP_Text missionFailedText;
    public float fadeDuration = 2f;

    [Header("Restart")]
    public float restartDelay = 3f;

    [Header("Mission Success")]
    public float successScreenDuration = 4f;
    public string introSceneName = "Intro";

    private bool endingStarted = false;

    private void Start()
    {
        if (successPanel != null)
            successPanel.SetActive(false);

        if (fadeOverlay != null)
        {
            fadeOverlay.color = new Color(0f, 0f, 0f, 0f);
            fadeOverlay.raycastTarget = false;
        }

        if (missionFailedText != null)
            missionFailedText.gameObject.SetActive(false);
    }

    public void TriggerPlayerDeath()
    {
        if (endingStarted)
            return;

        endingStarted = true;

        StopPlayerControls();

        Debug.Log("BK died! Mission failed.");

        StartCoroutine(FadeToMissionFailed());
    }

    private IEnumerator FadeToMissionFailed()
    {
        if (fadeOverlay != null)
        {
            fadeOverlay.gameObject.SetActive(true);
            fadeOverlay.raycastTarget = true;

            Color color = Color.black;
            color.a = 0f;
            fadeOverlay.color = color;

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
        }

        if (missionFailedText != null)
        {
            missionFailedText.text = "MISSION FAILED";
            missionFailedText.gameObject.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(restartDelay);

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void TriggerMissionSuccess()
    {
        if (endingStarted)
            return;

        endingStarted = true;

        StopPlayerControls();

        Debug.Log("Donald Dump defeated! Mission success.");

        if (successPanel != null)
            successPanel.SetActive(true);

        StartCoroutine(ReturnToIntro());
    }

    private IEnumerator ReturnToIntro()
    {
        yield return new WaitForSecondsRealtime(
            successScreenDuration
        );

        Time.timeScale = 1f;

        SceneLoader.LoadLevel(introSceneName);
    }

    private void StopPlayerControls()
    {
        foreach (MonoBehaviour script in playerControlScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }

        Time.timeScale = 0f;
    }
}
