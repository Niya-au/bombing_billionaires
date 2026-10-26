using UnityEngine;

public class PasswordAreaTrigger : MonoBehaviour
{
    [SerializeField] private GameObject passwordPanel;

    private void Start()
    {
        passwordPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            passwordPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            passwordPanel.SetActive(false);
        }
    }
}