
using UnityEngine;
using System.Collections;

public class DumpIntroDialogue : MonoBehaviour
{
    [Header("New Dialogue Image")]
    [SerializeField] private GameObject dumpDialogueImage;
    [SerializeField] private float dialogueDuration = 4f;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float triggerDistance = 4f;

    private DumpBoss dumpBoss;
    private bool introStarted;

    private void Start()
    {
        dumpBoss = GetComponent<DumpBoss>();

        if (dumpDialogueImage != null)
        {
            dumpDialogueImage.SetActive(false);
        }

        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void Update()
    {
        if (introStarted || player == null || dumpBoss == null)
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
        // Stop bossens handlinger under dialogen
        dumpBoss.SetDialogueActive(true);

        // Vis Donald Dumps nye dialogbillede
        if (dumpDialogueImage != null)
        {
            dumpDialogueImage.SetActive(true);
        }
        else
        {
            Debug.LogWarning(
                "DumpIntroDialogue: Dump Dialogue Image mangler!"
            );
        }

        // Vent, mens billedet vises
        yield return new WaitForSecondsRealtime(dialogueDuration);

        // Skjul billedet igen
        if (dumpDialogueImage != null)
        {
            dumpDialogueImage.SetActive(false);
        }

        // Fortsæt spillet og lad Donald Dump flygte
        dumpBoss.SetDialogueActive(false);
        dumpBoss.FinishIntroAndEscape();
    }
}
