using UnityEngine;

public class MarpDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueBox dialogueBox;

    [TextArea]
    [SerializeField]
    private string dialogueLine =
        "You really thought you could stop the upload?";

    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        if (hasPlayed)
            return;

        hasPlayed = true;

        dialogueBox.ShowLine(dialogueLine);
    }
}