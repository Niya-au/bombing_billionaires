using System.Collections;
using UnityEngine;
using TMPro;

public class Level1Intro : MonoBehaviour
{
    [SerializeField] private GameObject introGuide;
    [SerializeField] private GameObject xDialogueBox;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private MonoBehaviour playerMovement;

    [TextArea]
    [SerializeField] private string message =
        "So... you finally made it to Dump Tower?\nImpressive.\nBut this is where your little rebellion ends.";

    [SerializeField] private float typeSpeed = 0.04f;
    [SerializeField] private float readTime = 2f;

    private bool keepDialogueVisible = false;
    private bool waitingForEnter = false;

    private void Start()
    {
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (introGuide != null)
            introGuide.SetActive(false);

        if (xDialogueBox != null)
            xDialogueBox.SetActive(true);

        StartCoroutine(IntroSequence());
    }

    private void Update()
    {
        // Hold X-boksen synlig under hele intro-dialoguen
        if (keepDialogueVisible && xDialogueBox != null)
        {
            if (!xDialogueBox.activeSelf)
                xDialogueBox.SetActive(true);
        }

        if (waitingForEnter && Input.GetKeyDown(KeyCode.Return))
        {
            waitingForEnter = false;

            if (introGuide != null)
                introGuide.SetActive(false);

            if (playerMovement != null)
                playerMovement.enabled = true;
        }
    }

    private IEnumerator IntroSequence()
    {
        keepDialogueVisible = true;

        dialogueText.text = "";

        // Skriv hele teksten
        foreach (char letter in message)
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }

        // Lad den færdige tekst stå
        yield return new WaitForSecondsRealtime(readTime);

        // Nu må boksen forsvinde
        keepDialogueVisible = false;

        // Skift DIREKTE til guiden
        xDialogueBox.SetActive(false);
        introGuide.SetActive(true);

        waitingForEnter = true;
    }
}