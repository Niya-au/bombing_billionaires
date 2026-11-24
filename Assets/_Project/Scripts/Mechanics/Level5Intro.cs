using UnityEngine;

public class Level5Intro : MonoBehaviour
{
    public GameObject dialogueBox;
    public GameObject pressEnterUI;

    public MonoBehaviour playerMovement;

    private bool waitingForEnter = false;

    void Start()
    {
        pressEnterUI.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = false;
    }

    void Update()
    {
        if (waitingForEnter && Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
    }

    public void DialogueFinished()
    {
        dialogueBox.SetActive(false);

        pressEnterUI.SetActive(true);

        waitingForEnter = true;
    }

    void StartGame()
    {
        pressEnterUI.SetActive(false);

        waitingForEnter = false;

        if (playerMovement != null)
            playerMovement.enabled = true;
    }
}
