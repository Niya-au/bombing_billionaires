
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RiddleShield : MonoBehaviour
{
    [Header("Riddle")]
    public RiddleData riddle;

    [Header("UI")]
    public GameObject riddlePanel;
    public TMP_Text questionText;
    public Button[] answerButtons;
    public TMP_Text[] answerTexts;

    [Header("Wrong Answer Penalty")]
    public float lockoutTime = 2f;

    private Rigidbody2D playerRb;
    private bool isLocked;
    private bool isOpen;

    // The shield currently using the shared UI.
    private static RiddleShield activeRiddle;

    private void Start()
    {
        if (riddlePanel != null && activeRiddle == null)
            riddlePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OpenRiddle(other.GetComponent<Rigidbody2D>());
        }
    }

    public void OpenRiddle(Rigidbody2D rb)
    {
        if (isOpen || activeRiddle != null)
            return;

        if (riddle == null || riddlePanel == null ||
            questionText == null || answerButtons == null ||
            answerTexts == null)
        {
            Debug.LogError("RiddleShield is missing references!");
            return;
        }

        playerRb = rb;
        activeRiddle = this;
        isOpen = true;
        isLocked = false;

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.simulated = false;
        }

        questionText.text = riddle.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < riddle.answers.Length && i < answerTexts.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerTexts[i].text = riddle.answers[i];
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }

        // Connect buttons to the currently active riddle.
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answerIndex = i;

            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(
                () => CheckAnswer(answerIndex)
            );
        }

        riddlePanel.SetActive(true);
    }

    public void CheckAnswer(int answerIndex)
    {
        if (activeRiddle != this || !isOpen)
            return;

        if (isLocked)
        {
            Debug.Log("Wait! Riddle is still locked.");
            return;
        }

        if (answerIndex == riddle.correctAnswerIndex)
        {
            Debug.Log("Correct answer!");

            isOpen = false;
            activeRiddle = null;

            riddlePanel.SetActive(false);

            if (playerRb != null)
                playerRb.simulated = true;

            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Wrong answer! Locked for 2 seconds.");
            StartCoroutine(WrongAnswerLockout());
        }
    }

    private IEnumerator WrongAnswerLockout()
    {
        isLocked = true;

        yield return new WaitForSeconds(lockoutTime);

        isLocked = false;

        Debug.Log("Riddle unlocked. Try again!");
    }

    private void OnDisable()
    {
        if (activeRiddle == this)
        {
            activeRiddle = null;
            isOpen = false;
            isLocked = false;

            if (riddlePanel != null)
                riddlePanel.SetActive(false);

            if (playerRb != null)
                playerRb.simulated = true;
        }
    }
}
