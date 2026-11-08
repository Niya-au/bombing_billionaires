
using System.Collections;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    [Header("Camera")]
    public Transform mainCamera;
    public Transform jaffCameraPoint;
    public MonoBehaviour cameraFollowScript;
    public float cameraMoveDuration = 2f;

    [Header("Dialogue")]
    public DialogueBox dialogueBox;
    public float lineDuration = 4f;

    [Header("Cutscene Characters")]
    public GameObject cutsceneCharacters;

    [Header("Player Controls")]
    public MonoBehaviour[] playerControlScripts;

    [Header("Tutorial")]
    public LevelTutorial levelTutorial;

    private Vector3 originalCameraPosition;
    private bool cutsceneFinished = false;
    private Coroutine cutsceneRoutine;
    private static bool cutsceneAlreadyPlayed = false;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetCutsceneFlag()
    {
        cutsceneAlreadyPlayed = false;
    }

    private readonly string[] jaffLines =
    {
        "But, what happened to real innovation... ",
        "This civilization will only prosper by taking advantage of its resources.",
        "For the past 6 months, I have had the pleasure of developing:",
        "an army of robotic monkeys!",
        "Safe to say soon, this entire rainforest will become one enormous warehouse!",
        "No trees, no wildlife... just next-day delivery!",
        "I implore you to stop me BK"
    };


    void Start()
    {
        Time.timeScale = 1f;

        originalCameraPosition = mainCamera.position;

        if (levelTutorial != null &&
            levelTutorial.tutorialPanel != null)
        {
            levelTutorial.tutorialPanel.SetActive(false);
        }

        // Disable player controls until the tutorial is finished.
        foreach (MonoBehaviour script in playerControlScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        // If the player has already seen the cutscene,
        // skip directly to the tutorial.
        if (cutsceneAlreadyPlayed)
        {
            // Hide Jaff and the cutscene monkeys.
            if (cutsceneCharacters != null)
                cutsceneCharacters.SetActive(false);

            // Hide the cutscene dialogue.
            if (dialogueBox != null)
                dialogueBox.Hide();

            // Show the tutorial instead.
            if (levelTutorial != null)
                levelTutorial.ShowTutorial();

            return;
        }

        // First time playing: run the cutscene normally.
        if (cameraFollowScript != null)
            cameraFollowScript.enabled = false;

        cutsceneRoutine = StartCoroutine(PlayCutscene());
    }


    void Update()
    {
        // Press Enter to skip.
        if (!cutsceneFinished &&
            Input.GetKeyDown(KeyCode.Return))
        {
            if (cutsceneRoutine != null)
                StopCoroutine(cutsceneRoutine);

            FinishCutscene();
        }
    }

    IEnumerator PlayCutscene()
    {
        // Camera moves to Jaff.
        Vector3 target = new Vector3(
            jaffCameraPoint.position.x,
            jaffCameraPoint.position.y,
            originalCameraPosition.z
        );

        yield return MoveCamera(target);

        // Jaff speaks using your existing DialogueBox.
        foreach (string line in jaffLines)
        {
            dialogueBox.ShowLine(line);

            yield return new WaitForSecondsRealtime(lineDuration);
        }

        dialogueBox.Hide();

        // Camera returns to the player.
        yield return MoveCamera(originalCameraPosition);

        FinishCutscene();
    }

    IEnumerator MoveCamera(Vector3 target)
    {
        Vector3 start = mainCamera.position;
        float elapsed = 0f;

        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(
                elapsed / cameraMoveDuration
            );

            t = t * t * (3f - 2f * t);

            mainCamera.position = Vector3.Lerp(start, target, t);

            yield return null;
        }

        mainCamera.position = target;
    }


    void FinishCutscene()
    {
        if (cutsceneFinished)
            return;

        cutsceneFinished = true;
        cutsceneAlreadyPlayed = true;

        dialogueBox.Hide();

        mainCamera.position = originalCameraPosition;

        if (cutsceneCharacters != null)
            cutsceneCharacters.SetActive(false);

        if (cameraFollowScript != null)
            cameraFollowScript.enabled = true;

        Debug.Log("Cutscene finished!");

        if (levelTutorial != null)
        {
            Debug.Log("Calling ShowTutorial()");
            levelTutorial.ShowTutorial();
        }
        else
        {
            Debug.LogError("LevelTutorial is NOT assigned!");
        }
    }

}
