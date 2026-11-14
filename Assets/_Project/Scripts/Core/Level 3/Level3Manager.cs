using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Level3Manager : MonoBehaviour
{
    private enum Phase { CollectMoney, SurviveHazards, BossFight, Defeated }

    [Header("References")]
    [SerializeField] private MoneySpawner moneySpawner;
    [SerializeField] private HazardSpawner hazardSpawner;
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject alonBoss;

    [Header("UI")]
    [SerializeField] private TMP_Text phaseText;

    [Header("Settings")]
    [SerializeField, Min(1f)] private float hazardDuration = 15f;

    [Header("Boss Transition")]
    public UnityEvent OnBossFightStarted;

    private Phase phase;
    private float remainingTime;

    private void Start()
    {
        phase = Phase.CollectMoney;
        if (hazardSpawner != null) hazardSpawner.StopSpawning();
        if (moneySpawner != null) moneySpawner.SetSpawning(true);
        if (alonBoss != null) alonBoss.SetActive(false);
        SetPhaseText("COLLECT $1,000 TO CONTINUE!");
    }

    private void Update()
    {
        if (phase == Phase.Defeated) return;

        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            phase = Phase.Defeated;
            if (hazardSpawner != null) hazardSpawner.StopSpawning();
            if (moneySpawner != null) moneySpawner.SetSpawning(false);
            SetPhaseText("YOU DIED!");
            return;
        }

        if (phase == Phase.CollectMoney)
        {
            if (moneySpawner != null && moneySpawner.GoalReached)
                BeginHazards();
        }
        else if (phase == Phase.SurviveHazards)
        {
            remainingTime -= Time.deltaTime;
            SetPhaseText($"HAZARD PHASE\nSurvive {Mathf.CeilToInt(Mathf.Max(0f, remainingTime))} seconds!");
            if (remainingTime <= 0f) BeginBossFight();
        }
    }

    private void BeginHazards()
    {
        phase = Phase.SurviveHazards;
        remainingTime = hazardDuration;
        if (hazardSpawner != null) hazardSpawner.StartSpawning();
        // MoneySpawner stays active during hazards.
        Debug.Log("LEVEL 3: Hazard phase started; money keeps falling.");
    }

    private void BeginBossFight()
    {
        phase = Phase.BossFight;
        if (hazardSpawner != null)
        {
            hazardSpawner.StopSpawning();
            hazardSpawner.OnHazardPhaseComplete?.Invoke();
        }
        if (alonBoss != null) alonBoss.SetActive(true);
        SetPhaseText("ALON TRUSK - BOSS FIGHT!");
        OnBossFightStarted?.Invoke();
        Debug.Log("LEVEL 3: Boss fight started; money keeps falling.");
    }

    private void SetPhaseText(string message)
    {
        if (phaseText != null) phaseText.text = message;
    }
}
