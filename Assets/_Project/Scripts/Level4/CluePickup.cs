using UnityEngine;

public class CluePickup : MonoBehaviour
{
    [SerializeField] private string clueId;
    [SerializeField] private string revealedCharacter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        ClueManager.Instance.AddClue(clueId, revealedCharacter);

        gameObject.SetActive(false);
    }
}