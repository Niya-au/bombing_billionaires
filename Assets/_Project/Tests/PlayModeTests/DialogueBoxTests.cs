using System.Collections;
using System.Reflection;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class DialogueBoxTests
{
    [UnityTest]
    public IEnumerator DialogueBox_TypesTextOverTime_AndCanHide()
    {
        GameObject panel = new GameObject("DialoguePanel");

        GameObject textObject = new GameObject("DialogueText");
        TMP_Text text = textObject.AddComponent<TextMeshProUGUI>();

        GameObject dialogueObject = new GameObject("DialogueBox");
        DialogueBox dialogueBox = dialogueObject.AddComponent<DialogueBox>();

        SetPrivateField(dialogueBox, "dialoguePanel", panel);
        SetPrivateField(dialogueBox, "dialogueText", text);
        SetPrivateField(dialogueBox, "typingSpeed", 0.02f);

        panel.SetActive(false);

        dialogueBox.ShowLine("Hello");

        // Panelet skal vises med det samme
        Assert.IsTrue(panel.activeSelf);

        // Hele teksten bør ikke være færdig med det samme
        Assert.AreNotEqual("Hello", text.text);

        yield return new WaitForSeconds(0.15f);

        // Efter lidt tid skal teksten være færdig
        Assert.AreEqual("Hello", text.text);

        dialogueBox.Hide();

        // Hide() skal skjule panelet
        Assert.IsFalse(panel.activeSelf);

        Object.Destroy(panel);
        Object.Destroy(textObject);
        Object.Destroy(dialogueObject);
    }

    private void SetPrivateField(object target, string fieldName, object value)
    {
        FieldInfo field = target.GetType().GetField(
            fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        field.SetValue(target, value);
    }
}