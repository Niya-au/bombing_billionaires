using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    public static ClueManager Instance { get; private set; }

    [SerializeField] private TMP_Text collectedCluesText;

    private Dictionary<string, string> foundClues =
        new Dictionary<string, string>();

    private List<string> collectedCharacters =
        new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateClueText();
    }

    public void AddClue(string id, string revealedCharacter)
    {
        if (!foundClues.ContainsKey(id))
        {
            foundClues.Add(id, revealedCharacter);

            collectedCharacters.Add(revealedCharacter);

            Debug.Log(
                "Clue found: " + id +
                " | Reveals: " + revealedCharacter
            );

            UpdateClueText();
        }
    }

    private void UpdateClueText()
    {
        if (collectedCluesText != null)
        {
            collectedCluesText.text =
                "Clues: " + string.Join(" ", collectedCharacters);
        }
    }

    public bool HasClue(string id)
    {
        return foundClues.ContainsKey(id);
    }

    public int GetClueCount()
    {
        return foundClues.Count;
    }
}