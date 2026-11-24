using System.Collections;
using UnityEngine;
using TMPro;

public class Level4Intro : MonoBehaviour
{
    public GameObject xDialogueBox;
    public TMP_Text dialogueText;
    public GameObject[] Intro;

    public MonoBehaviour playerMovement;

    [TextArea]
    public string message = "Still digging, Tyrone? You're going to regret that.";

    public float typeSpeed = 0.08f;

    void Start()
    {
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (xDialogueBox != null)
            xDialogueBox.SetActive(false);

        foreach (GameObject introTrigger in Intro)
        {
            if (introTrigger != null)
                introTrigger.SetActive(false);
        }

        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        // Vis Marps X-dialogue
        xDialogueBox.SetActive(true);

        dialogueText.text = "";

        foreach (char letter in message)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }

        // Vent efter Marps tekst
        yield return new WaitForSeconds(2f);

        // Skjul Marps X-dialogue
        xDialogueBox.SetActive(false);

        // Giv spilleren movement tilbage
        if (playerMovement != null)
            playerMovement.enabled = true;

        // Tænd IntroTrigger
        foreach (GameObject introTrigger in Intro)
        {
            if (introTrigger != null)
                introTrigger.SetActive(true);
        }

        // IntroTrigger er aktiv i 2 sekunder
        yield return new WaitForSeconds(2f);

        // Skjul dialogue OG disable triggeren
        foreach (GameObject introTrigger in Intro)
        {
            if (introTrigger != null)
            {
                DialogueBox box = introTrigger.GetComponent<DialogueBox>();

                if (box != null)
                    box.Hide();

                introTrigger.SetActive(false);
            }
        }
    }
}