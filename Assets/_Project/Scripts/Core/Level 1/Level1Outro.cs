using System.Collections;
using UnityEngine;
using TMPro;

public class Level1Outro : MonoBehaviour
{
    [SerializeField] private GameObject outroDialogueBox;
    [SerializeField] private TMP_Text outroTekst;

    [TextArea]
    [SerializeField] private string message =
        "So... you actually found my office.\nYou've come this far.\nNow let's see if you can handle what's waiting for you.";

    [SerializeField] private float typeSpeed = 0.04f;
    [SerializeField] private float readTime = 2f;

    private bool outroStarted = false;
    private bool keepVisible = false;

    private void Update()
    {
        // Når noget andet tænder outro-boksen,
        // starter vi automatisk typing.
        if (!outroStarted && outroDialogueBox.activeSelf)
        {
            outroStarted = true;
            StartCoroutine(OutroSequence());
        }

        // Hold boksen synlig indtil teksten er færdig
        if (keepVisible && !outroDialogueBox.activeSelf)
        {
            outroDialogueBox.SetActive(true);
        }
    }

    private IEnumerator OutroSequence()
    {
        keepVisible = true;

        outroTekst.text = "";

        foreach (char letter in message)
        {
            outroTekst.text += letter;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }

        // Nu er hele teksten skrevet
        yield return new WaitForSecondsRealtime(readTime);

        keepVisible = false;

        // Nu må boksen forsvinde
        outroDialogueBox.SetActive(false);
    }
}