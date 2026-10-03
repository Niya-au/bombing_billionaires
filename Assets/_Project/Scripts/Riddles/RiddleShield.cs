using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RiddleShield : MonoBehaviour
{
    public RiddleData riddle;

    public GameObject riddlePanel;
    public TMP_Text questionText;

    public Button[] answerButtons;
    public TMP_Text[] answerTexts;

    private Rigidbody2D playerRb;

    private void Start()
    {
        riddlePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerRb = other.GetComponent<Rigidbody2D>();

            // Stop player movement while the riddle is open
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                playerRb.simulated = false;
            }

            OpenRiddle();
        }
    }

    private void OpenRiddle()
    {
        questionText.text = riddle.question;

        for (int i = 0; i < answerTexts.Length; i++)
        {
            answerTexts[i].text = riddle.answers[i];
        }

        riddlePanel.SetActive(true);
    }

    public void CheckAnswer(int answerIndex)
    {
        if (answerIndex == riddle.correctAnswerIndex)
        {
            Debug.Log("Correct answer!");

            riddlePanel.SetActive(false);

            // Give player control back
            if (playerRb != null)
            {
                playerRb.simulated = true;
            }

            // Disable the Riddle Shield
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Wrong answer!");
        }
    }
}