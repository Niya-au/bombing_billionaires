
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BackstoryController : MonoBehaviour
{
    [System.Serializable]
    public class BackstoryScene
    {
        public Sprite image;

        [TextArea(3, 6)]
        public string narration;

        public float duration = 6f;
    }

    [Header("Backstory Scenes")]
    [SerializeField] private BackstoryScene[] scenes;

    [Header("UI References")]
    [SerializeField] private Image storyImage;
    [SerializeField] private TMP_Text narrationText;
    [SerializeField] private Image fadeOverlay;

    [Header("Transition Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float typingSpeed = 0.035f;

    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "LevelSelect";

    private bool sequenceFinished;

    private void Start()
    {
        Time.timeScale = 1f;

        if (storyImage == null ||
            narrationText == null ||
            fadeOverlay == null)
        {
            Debug.LogError("Backstory UI references are missing!");
            return;
        }

        if (scenes == null || scenes.Length == 0)
        {
            Debug.LogWarning("No backstory scenes assigned!");
            FinishBackstory();
            return;
        }

        narrationText.text = "";

        fadeOverlay.color = Color.black;
        fadeOverlay.raycastTarget = false;

        StartCoroutine(PlayBackstory());
    }

    private IEnumerator PlayBackstory()
    {
        foreach (BackstoryScene scene in scenes)
        {
            // Display the next illustration.
            storyImage.sprite = scene.image;

            narrationText.text = "";

            // Fade in from black.
            yield return StartCoroutine(Fade(1f, 0f));

            // Display narration with a typewriter effect.
            yield return StartCoroutine(
                TypeNarration(scene.narration)
            );

            // Keep the scene visible.
            yield return new WaitForSecondsRealtime(
                scene.duration
            );

            // Fade to black before the next scene.
            yield return StartCoroutine(Fade(0f, 1f));
        }

        FinishBackstory();
    }

    private IEnumerator TypeNarration(string narration)
    {
        narrationText.text = narration;
        narrationText.maxVisibleCharacters = 0;

        narrationText.ForceMeshUpdate();

        int characterCount =
            narrationText.textInfo.characterCount;

        for (int i = 1; i <= characterCount; i++)
        {
            narrationText.maxVisibleCharacters = i;

            yield return new WaitForSecondsRealtime(
                typingSpeed
            );
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        Color color = Color.black;
        color.a = startAlpha;
        fadeOverlay.color = color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(
                elapsed / Mathf.Max(fadeDuration, 0.01f)
            );

            color.a = Mathf.Lerp(
                startAlpha,
                endAlpha,
                progress
            );

            fadeOverlay.color = color;

            yield return null;
        }

        color.a = endAlpha;
        fadeOverlay.color = color;
    }

    public void SkipBackstory()
    {
        FinishBackstory();
    }

    private void FinishBackstory()
    {
        if (sequenceFinished)
            return;

        sequenceFinished = true;

        StopAllCoroutines();

        Time.timeScale = 1f;

        SceneLoader.LoadLevel(nextSceneName);
    }
}
