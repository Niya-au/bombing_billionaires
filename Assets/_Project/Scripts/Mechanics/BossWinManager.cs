
using System.Collections;
using UnityEngine;

public class BossWinManager : MonoBehaviour
{
    [Header("Mission Success UI")]
    public GameObject successPanel;

    [Header("Player Controls")]
    public MonoBehaviour[] playerControlScripts;

    [Header("Mission Success")]
    public float successScreenDuration = 4f;

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

        // Start the return-to-level select sequence.
        StartCoroutine(ReturnToLevelSelect());

    }

    private IEnumerator ReturnToLevelSelect()
    {
        // Keep the SuccessPanel visible for 4 seconds.
        yield return new WaitForSecondsRealtime(successScreenDuration);

        // Restore normal game speed.
        Time.timeScale = 1f;

        // Load the Intro scene.
        SceneLoader.LoadLevel("LevelSelect");
    }
}
