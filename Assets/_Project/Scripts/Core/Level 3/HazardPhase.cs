using UnityEngine;
using TMPro;

public class HazardPhase : MonoBehaviour
{
    [Header("Level 3")]
    [SerializeField] private MoneySpawner moneySpawner;
    [SerializeField] private HazardSpawner hazardSpawner;
    [SerializeField] private Health playerHealth;

    [Header("UI")]
    [SerializeField] private TMP_Text phaseText;

    [Header("Settings")]
    [SerializeField] private float hazardDuration = 15f;

    private float timer = 0f;
    private bool phaseStarted = false;
    private bool phaseFinished = false;

    private void Start()
    {
        if (phaseText != null)
        {
            phaseText.text = "";
        }
    }

    private void Update()
    {
        // Vent på $1000
        if (moneySpawner == null)
            return;

        if (!moneySpawner.GoalReached)
            return;

        // Start hazard-fasen
        if (!phaseStarted)
        {
            StartHazardPhase();
        }

        if (phaseFinished)
            return;

        // Spilleren døde
        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            PlayerDied();
            return;
        }

        // Tæl tiden
        timer += Time.deltaTime;

        // Vis countdown
        if (phaseText != null)
        {
            int secondsLeft = Mathf.CeilToInt(hazardDuration - timer);

            phaseText.text =
                "HAZARD PHASE\n" +
                "Survive " + secondsLeft + " seconds!";
        }

        // Spilleren overlevede
        if (timer >= hazardDuration)
        {
            CompleteHazardPhase();
        }
    }

    private void StartHazardPhase()
    {
        phaseStarted = true;
        timer = 0f;

        Debug.Log("HAZARD PHASE STARTED!");

        if (phaseText != null)
        {
            phaseText.text =
                "HAZARD PHASE\n" +
                "Survive 15 seconds!";
        }
    }

    private void PlayerDied()
    {
        phaseFinished = true;

        if (hazardSpawner != null)
        {
            hazardSpawner.StopSpawning();
        }

        if (phaseText != null)
        {
            phaseText.text =
                "YOU DIED";
        }

        Debug.Log("PLAYER DIED DURING HAZARD PHASE!");
    }

    private void CompleteHazardPhase()
    {
        phaseFinished = true;

        if (hazardSpawner != null)
        {
            hazardSpawner.StopSpawning();
        }

        if (phaseText != null)
        {
            phaseText.text =
                "HAZARD PHASE COMPLETE!\n" +
                "BOSS FIGHT!";
        }

        Debug.Log("HAZARD PHASE COMPLETE!");
        Debug.Log("PLAYER SURVIVED 15 SECONDS!");

        // Her kobler vi bossen på bagefter.
    }
}