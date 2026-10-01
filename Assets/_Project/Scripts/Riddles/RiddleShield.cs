using UnityEngine;
using TMPro;

public class RiddleShield : MonoBehaviour
{
    public RiddleData riddle;
    public GameObject riddlePanel;
    public TMP_Text questionText;

    private void Start()
    {
        riddlePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            questionText.text = riddle.question;
            riddlePanel.SetActive(true);
        }
    }

    public void CheckAnswer(string answer)
    {
        if (answer.Trim().ToLower() == riddle.correctAnswer.Trim().ToLower())
        {
            Debug.Log("Correct answer!");

            riddlePanel.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Wrong answer!");
        }
    }
}