using System.Collections.Generic;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    public static ClueManager Instance { get; private set; }

    private Dictionary<string, string> foundClues =
        new Dictionary<string, string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddClue(string id, string revealedCharacter)
    {
        if (!foundClues.ContainsKey(id))
        {
            foundClues.Add(id, revealedCharacter);

            Debug.Log(
                "Clue found: " + id +
                " | Reveals: " + revealedCharacter
            );
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