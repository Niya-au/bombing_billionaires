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
    private bool introStarted;

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
        if (dumpBoss != null)
            dumpBoss.SetDialogueActive(true);

        if (dialogueBox != null)
        {
            dialogueBox.ShowLine(
                "Donald Dump: Nobody beats me! Not even China! You're fired!"
            );

            yield return new WaitForSeconds(dialogueDuration);

            dialogueBox.Hide();
        }

        if (dumpBoss != null)
        {
            dumpBoss.SetDialogueActive(false);
            dumpBoss.FinishIntroAndEscape();
        }
    }
}