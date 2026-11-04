
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTest : MonoBehaviour
{
    [SerializeField] private DialogueBox dialogueBox;

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("E-tasten virker!");

            if (dialogueBox != null)
            {
                dialogueBox.ShowLine("Test Dialogue");
            }
            else
            {
                Debug.LogError("DialogueBox mangler i Inspector!");
            }
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (dialogueBox != null)
            {
                dialogueBox.Hide();
            }
        }
    }
}
