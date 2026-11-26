
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Level4EndingController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Health tyroneHealth;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private MonoBehaviour[] playerControlScripts;

    [Header("Boss")]
    [SerializeField] private GameObject boss;

    [Header("Mission Success")]
    [SerializeField] private GameObject successPanel;
    [SerializeField] private float successScreenDuration = 4f;

    [Header("Mission Failed")]
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private TMP_Text missionFailedText;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float restartDelay = 3f;

    private bool endingStarted = false;
    private bool bossWasAssigned;

    private void Start()
    {
        Time.timeScale = 1f;

        bossWasAssigned = boss != null;

        if (!bossWasAssigned)
            Debug.LogError("Level 4 boss is not assigned!");

        if (successPanel != null)
            successPanel.SetActive(false);

        if (fadeOverlay != null)
        {
            fadeOverlay.color = new Color(0f, 0f, 0f, 0f);
            fadeOverlay.raycastTarget = false;
            fadeOverlay.gameObject.SetActive(false);
        }

        if (missionFailedText != null)
            missionFailedText.gameObject.SetActive(false);

        // Subscribe to BK's death event.
        if (tyroneHealth != null)
        {
            tyroneHealth.OnDied.AddListener(
                TriggerPlayerDeath
            );
        }
        else
        {
            Debug.LogError("Player Health is not assigned!");
        }
    }

    private void Update()
    {
        if (endingStarted)
            return;

        // Detect when Marp has been destroyed.
        if (bossWasAssigned && boss == null)
        {
            TriggerMissionSuccess();
        }
    }

    private void OnDestroy()
    {
        if (tyroneHealth != null)
        {
            tyroneHealth.OnDied.RemoveListener(
                TriggerPlayerDeath
            );
        }
    }

    // Called when BK dies.
    public void TriggerPlayerDeath()
    {
        if (endingStarted)
            return;

        endingStarted = true;

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

        yield return new WaitForSecondsRealtime(
            restartDelay
        );

        // Restart Level 4.
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // Called when Marp Suckerberg is defeated.
    public void TriggerMissionSuccess()
    {
        if (endingStarted)
            return;

        endingStarted = true;

        Debug.Log("Marp Suckerberg defeated! Mission success.");

        // Unlock Level 5.
        LevelProgression.CompleteLevel(3);

        StopPlayerControls();

        if (successPanel != null)
            successPanel.SetActive(true);
        else
            Debug.LogWarning("SuccessPanel is not assigned!");

        StartCoroutine(ReturnToLevelSelect());
    }

    private IEnumerator ReturnToLevelSelect()
    {
        yield return new WaitForSecondsRealtime(
            successScreenDuration
        );

        Time.timeScale = 1f;

        SceneLoader.LoadLevel("LevelSelect");
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
}
