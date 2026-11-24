using UnityEngine;

public class Level4Win : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject winUI;

    private bool hasWon = false;

    private void Start()
    {
        if (winUI != null)
            winUI.SetActive(false);
    }

    private void Update()
    {
        if (hasWon)
            return;

        if (boss == null)
        {
            hasWon = true;

            if (winUI != null)
                winUI.SetActive(true);

            Time.timeScale = 0f;
        }
    }
}