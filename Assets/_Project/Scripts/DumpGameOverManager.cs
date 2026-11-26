
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DumpGameOverManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private MonoBehaviour[] playerControlScripts;
    [SerializeField] private Rigidbody2D playerRb;

    [Header("Mission Success")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private float successScreenDuration = 4f;
    [SerializeField] private string introSceneName = "LevelSelect";

    [Header("Mission Failed")]
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private TMP_Text missionFailedText;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float restartDelay = 3f;

    private bool gameFinished;

    private void Start()
    {
        Time.timeScale = 1f;
        gameFinished = false;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (fadeOverlay != null)
        {
            fadeOverlay.color = new Color(0f, 0f, 0f, 0f);
            fadeOverlay.raycastTarget = false;
            fadeOverlay.gameObject.SetActive(false);
        }

        if (missionFailedText != null)
            missionFailedText.gameObject.SetActive(false);
    }

    // Called when BK dies.
    public void ShowGameOver()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        Debug.Log("BK died! Mission failed.");

        StopPlayerControls();

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
        else
        {
            Debug.LogWarning("FadeOverlay is not assigned!");
        }

        // Display Mission Failed.
        if (missionFailedText != null)
        {
            missionFailedText.text = "MISSION FAILED";
            missionFailedText.gameObject.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(restartDelay);

        PlayAgain();
    }

    // Called when Donald Dump is defeated.
    public void ShowVictory()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        Debug.Log("Donald Dump defeated! Mission success.");

        // Save Level 1 completion and unlock Level 2.
        LevelProgression.CompleteLevel(1);

        StopPlayerControls();

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

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

    public void PlayAgain()
    {
        StopAllCoroutines();

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    private void StopPlayerControls()
    {
        if (playerControlScripts != null)
        {
            foreach (MonoBehaviour script in playerControlScripts)
            {
                if (script != null)
                    script.enabled = false;
            }
        }

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }

        Time.timeScale = 0f;
    }

    // Compatibility with ending controller methods.
    public void TriggerPlayerDeath()
    {
        ShowGameOver();
    }

    public void TriggerMissionSuccess()
    {
        ShowVictory();
    }
}
