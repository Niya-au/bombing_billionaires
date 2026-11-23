
using UnityEngine;

public class BossWinManager : MonoBehaviour
{
    [Header("Mission Success UI")]
    public GameObject successPanel;

    [Header("Player Controls")]
    public MonoBehaviour[] playerControlScripts;

    private bool bossDefeated = false;

    private void Start()
    {
        if (successPanel != null)
            successPanel.SetActive(false);
    }

    public void BossDefeated()
    {
        if (bossDefeated)
            return;

        bossDefeated = true;

        Debug.Log("BOSS DEFEATED! MISSION SUCCESS!");

        // Disable BK's controls.
        foreach (MonoBehaviour script in playerControlScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        // Show the success panel.
        if (successPanel != null)
        {
            successPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Success Panel is not assigned!");
        }

        // Pause gameplay while the success panel is visible.
        Time.timeScale = 0f;
    }
}
