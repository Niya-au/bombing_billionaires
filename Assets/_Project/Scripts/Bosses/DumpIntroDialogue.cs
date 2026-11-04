
using UnityEngine;
using System.Collections;

public class DumpIntroDialogue : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private float dialogueDuration = 4f;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float triggerDistance = 4f;

    private DumpBoss dumpBoss;
    private bool introStarted = false;

    private void Start()
    {
        dumpBoss = GetComponent<DumpBoss>();

        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
                player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (introStarted || player == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance <= triggerDistance)
        {
            introStarted = true;
            StartCoroutine(PlayIntro());
        }
    }

    private IEnumerator PlayIntro()
    {
        if (dialogueBox != null)
        {
            dialogueBox.ShowLine(
                "Donald Dump: Welcome, BK! Think you can defeat me?"
            );

            yield return new WaitForSeconds(dialogueDuration);

            dialogueBox.Hide();
        }
        else
        {
            Debug.LogWarning("DialogueBox is missing!");
        }

        // Start Riddle01 after dialogue
        if (dumpBoss != null)
        {
            dumpBoss.StartFirstRiddle();
        }
    }
}
