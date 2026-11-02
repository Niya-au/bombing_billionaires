using UnityEngine;

public class BossWinManager : MonoBehaviour
{
    public GameObject winPanel;

    public void BossDefeated()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}