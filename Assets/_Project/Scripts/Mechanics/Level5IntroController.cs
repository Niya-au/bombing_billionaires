using System.Collections;
using UnityEngine;
using TMPro;

public class Level5IntroController : MonoBehaviour
{
    public GameObject introGuide;
    public GameObject xDialogueBox;
    public TMP_Text dialogueText;
    public GameObject pressEnterUI;
    public GameObject[] drones;
    
    public MonoBehaviour playerMovement;

    [TextArea]
    public string message = "You found my evidence... now come face me.";

    public float typeSpeed = 0.04f;

    private bool waitingForEnter = false;

    void Start()
{
    playerMovement.enabled = false;

    xDialogueBox.SetActive(false);
    pressEnterUI.SetActive(false);

    StartCoroutine(IntroSequence());

    foreach (GameObject drone in drones)
    {
        if (drone != null)
            drone.SetActive(false);
    }
}

 IEnumerator IntroSequence()
{
    // Først X dialogue
    xDialogueBox.SetActive(true);

    dialogueText.text = "";

    foreach (char letter in message)
    {
        dialogueText.text += letter;
        yield return new WaitForSeconds(typeSpeed);
    }

    // Lille pause efter teksten
    yield return new WaitForSeconds(1.5f);

    // Skjul X dialogue
    xDialogueBox.SetActive(false);

    // Vis den store guide
    introGuide.SetActive(true);

    // Vis Press Enter
    pressEnterUI.SetActive(true);

    waitingForEnter = true;
}

    void Update()
    {
        if (waitingForEnter && Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        waitingForEnter = false;

        if (xDialogueBox != null)
            xDialogueBox.SetActive(false);

        if (pressEnterUI != null)
            pressEnterUI.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (introGuide != null)
            introGuide.SetActive(false);
        
        foreach (GameObject drone in drones)
            {
                if (drone != null)
                    drone.SetActive(true);
            }
    }
}
