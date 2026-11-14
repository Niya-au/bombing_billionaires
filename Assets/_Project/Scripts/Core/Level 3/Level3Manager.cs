using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Level3Manager : MonoBehaviour
{
    private enum Phase
    {
        CollectMoney,
        SurviveHazards,
        BossEntrance,
        BossFight,
        Defeated
    }

    [Header("References")]
    [SerializeField] private MoneySpawner moneySpawner;
    [SerializeField] private HazardSpawner hazardSpawner;
    [SerializeField] private Health playerHealth;
    [SerializeField] private AlonTruskBoss alonBoss;

    [Header("Entrance")]
    [SerializeField] private GameObject chrysalisVisual;
    [SerializeField] private GameObject alonVisual;
    [SerializeField] private Transform entranceStart;
    [SerializeField] private Transform entranceEnd;
    [SerializeField] private Collider2D[] bossColliders;
    [SerializeField] private Animator chrysalisAnimator;
    [SerializeField, Min(0.1f)] private float hatchDuration = 1f;

    [SerializeField, Min(0.1f)] private float flyInDuration = 2f;
    [SerializeField, Min(0f)] private float shakeDuration = 1f;
    [SerializeField, Min(0f)] private float shakeAmount = 0.06f;

    [Header("UI")]
    [SerializeField] private TMP_Text phaseText;
    [SerializeField] private GameObject bossHealthUI;

    [Header("Settings")]
    [SerializeField, Min(1f)] private float hazardDuration = 15f;

    [Header("Boss Transition")]
    public UnityEvent OnBossFightStarted;

    private Phase phase;
    private float remainingTime;
    private Coroutine entranceRoutine;

    private void Start()
    {
        phase = Phase.CollectMoney;

        if (hazardSpawner != null)
            hazardSpawner.StopSpawning();

        if (moneySpawner != null)
            moneySpawner.SetSpawning(true);

        if (alonBoss != null)
            alonBoss.gameObject.SetActive(false);

        if (bossHealthUI != null)
            bossHealthUI.SetActive(false);

        SetPhaseText("COLLECT $1,000 TO CONTINUE!");
    }

    private void Update()
    {
        if (phase == Phase.Defeated)
            return;

        if (playerHealth != null && playerHealth.currentHP <= 0)
        {
            HandlePlayerDeath();
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

            SetPhaseText(
                $"HAZARD PHASE\nSurvive " +
                $"{Mathf.CeilToInt(Mathf.Max(0f, remainingTime))} seconds!"
            );

            if (remainingTime <= 0f)
                BeginBossEntrance();
        }
    }

    private void BeginHazards()
    {
        phase = Phase.SurviveHazards;
        remainingTime = hazardDuration;

        if (hazardSpawner != null)
            hazardSpawner.StartSpawning();
    }

    private void BeginBossEntrance()
    {
        phase = Phase.BossEntrance;

        if (hazardSpawner != null)
            hazardSpawner.StopSpawning();

        // Do not invoke OnHazardPhaseComplete here:
        // existing listeners could start combat too early.

        if (alonBoss == null ||
            chrysalisVisual == null ||
            alonVisual == null ||
            entranceStart == null ||
            entranceEnd == null)
        {
            Debug.LogError(
                "Level3Manager: Assign all boss entrance references.",
                this
            );
            enabled = false;
            return;
        }

        entranceRoutine = StartCoroutine(PlayBossEntrance());
    }

    private IEnumerator PlayBossEntrance()
    {
        SetPhaseText("SOMETHING IS APPROACHING...");

        Vector3 from = entranceStart.position;
        Vector3 to = entranceEnd.position;

        alonBoss.transform.position = from;
        alonBoss.gameObject.SetActive(true);
        alonBoss.StopBossFight();

        SetBossColliders(false);
        chrysalisVisual.SetActive(true);
        alonVisual.SetActive(false);

        float elapsed = 0f;

        while (elapsed < flyInDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / flyInDuration);
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            alonBoss.transform.position =
                Vector3.Lerp(from, to, easedProgress);

            yield return null;
        }

        alonBoss.transform.position = to;
        SetPhaseText("THE CHRYSALIS IS OPENING...");

        Vector3 originalLocalPosition =
            chrysalisVisual.transform.localPosition;

        elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            Vector2 offset = Random.insideUnitCircle * shakeAmount;

            chrysalisVisual.transform.localPosition =
                originalLocalPosition +
                new Vector3(offset.x, offset.y, 0f);

            yield return null;
        }

        chrysalisVisual.transform.localPosition = originalLocalPosition;

        // Play the emergence animation before enabling combat.
        if (chrysalisAnimator == null)
        {
            chrysalisAnimator = chrysalisVisual.GetComponent<Animator>();
        }

        if (chrysalisAnimator == null ||
            !chrysalisAnimator.isActiveAndEnabled ||
            chrysalisAnimator.runtimeAnimatorController == null)
        {
            Debug.LogError(
                "Chrysalis needs an enabled Animator with a Controller assigned.",
                chrysalisVisual
            );

            yield break;
        }

        Debug.Log("Triggering chrysalis Hatch animation.", chrysalisVisual);

        chrysalisAnimator.SetTrigger("Hatch");
        yield return new WaitForSeconds(hatchDuration);

        // Replace the animated emergence sprite with the combat visual.
        chrysalisVisual.SetActive(false);
        alonVisual.SetActive(true);

        SetBossColliders(true);

        if (bossHealthUI != null)
            bossHealthUI.SetActive(true);

        phase = Phase.BossFight;
        SetPhaseText("Ah, you've dodged my spacetruck...Well, then, let's see how you fare against me! ALON TRUSK — BOSS FIGHT!");

        alonBoss.StartBossFight();
        OnBossFightStarted?.Invoke();

        entranceRoutine = null;
    }

    private void HandlePlayerDeath()
    {
        phase = Phase.Defeated;

        if (entranceRoutine != null)
        {
            StopCoroutine(entranceRoutine);
            entranceRoutine = null;
        }

        if (hazardSpawner != null)
            hazardSpawner.StopSpawning();

        if (moneySpawner != null)
            moneySpawner.SetSpawning(false);

        if (alonBoss != null)
        {
            alonBoss.StopBossFight();
            alonBoss.gameObject.SetActive(false);
        }

        if (bossHealthUI != null)
            bossHealthUI.SetActive(false);

        SetPhaseText("YOU DIED!");
    }

    private void SetBossColliders(bool active)
    {
        if (bossColliders == null)
            return;

        foreach (Collider2D bossCollider in bossColliders)
        {
            if (bossCollider != null)
                bossCollider.enabled = active;
        }
    }

    private void SetPhaseText(string message)
    {
        if (phaseText != null)
            phaseText.text = message;
    }
}