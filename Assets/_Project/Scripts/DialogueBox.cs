using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    public void ShowLine(string text)
    {
        dialogueText.text = text;
        dialoguePanel.SetActive(true);
    }

    public void Hide()
    {
        dialoguePanel.SetActive(false);
    }
}