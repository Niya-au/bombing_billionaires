using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTest : MonoBehaviour
{
    [SerializeField] private DialogueBox dialogueBox;

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            dialogueBox.ShowLine("Test Dialogue");
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            dialogueBox.Hide();
        }
    }
} //hello