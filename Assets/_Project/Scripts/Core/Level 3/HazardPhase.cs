using UnityEngine;
using TMPro;

public class HazardPhase : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MoneySpawner moneySpawner;
    [SerializeField] private HazardSpawner hazardSpawner;
    [SerializeField] private GameObject boss;
    [SerializeField] private TMP_Text timerText;

    [Header("Survival Settings")]
    [SerializeField] private float survivalTime = 15f;

    private float timer;
    private bool phaseStarted;
    private bool phaseFinished;

    private void Start()
    {
        timer = survivalTime;

        if (timerText != null)
        {
            timerText.text = "";
        }

        // Boss starter skjult
        if (boss != null)
        {
            boss.SetActive(false);
        }
    }

    private void Update()
    {
        if (phaseFinished)
            return;

        // Vent på at spilleren har samlet $1000
        if (!phaseStarted)
        {
            if (moneySpawner == null || !moneySpawner.GoalReached)
                return;

            StartHazardPhase();
        }

        timer -= Time.deltaTime;

        if (timer < 0f)
            timer = 0f;

        UpdateTimerUI();

        // Survival-fasen er færdig
        if (timer <= 0f)
        {
            FinishHazardPhase();
        }
    }

    private void StartHazardPhase()
    {
        phaseStarted = true;
        timer = survivalTime;

        Debug.Log("⚠️ HAZARD PHASE STARTED!");

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "SURVIVE: " + Mathf.CeilToInt(timer);
        }
    }

    private void FinishHazardPhase()
    {
        phaseFinished = true;

        Debug.Log("✅ HAZARD PHASE COMPLETE!");

        // Stop hazards
        if (hazardSpawner != null)
        {
            hazardSpawner.StopSpawning();
        }

        if (timerText != null)
        {
            timerText.text = "BOSS INCOMING!";
        }

        // Start boss
        if (boss != null)
        {
            boss.SetActive(true);
        }
    }
}