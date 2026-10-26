
using UnityEngine;

public class DumpBoss : BossBase
{
    [Header("Animator")]
    [SerializeField] private Animator bossAnimator;

    [Header("Riddle Shields")]
    [SerializeField] private GameObject riddleShield;
    [SerializeField] private GameObject riddleShield02;
    [SerializeField] private GameObject riddlePanel;

    [Header("Player")]
    [SerializeField] private Rigidbody2D playerRb;

    [Header("Damage Test")]
    [SerializeField] private bool enableDamageTest = true;
    [SerializeField] private int testDamage = 10;

    private bool shieldProtectingBoss;
    private bool isDefeated;
    private int riddleStep;

    protected override void Start()
    {
        base.Start();

        isDefeated = false;
        riddleStep = 1;
        shieldProtectingBoss = true;

        // Find Animator automatically if not assigned.
        if (bossAnimator == null)
            bossAnimator = GetComponent<Animator>();

        if (riddleShield02 != null)
            riddleShield02.SetActive(false);

        if (riddlePanel != null)
            riddlePanel.SetActive(false);

        // Open Riddle01 automatically at 100 HP.
        if (riddleShield != null)
        {
            riddleShield.SetActive(true);

            RiddleShield firstRiddle =
                riddleShield.GetComponent<RiddleShield>();

            if (firstRiddle != null)
            {
                firstRiddle.OpenRiddle(playerRb);
                Debug.Log("100 HP: Riddle01 opened!");
            }
        }
        else
        {
            Debug.LogWarning("RiddleShield01 is missing!");
        }

        Debug.Log("DumpBoss started with " + currentHP + " HP");
    }

    private void Update()
    {
        if (isDefeated || currentHP <= 0)
            return;

        // Test damage by pressing H.
        if (enableDamageTest && Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(testDamage);

            Debug.Log(
                "DumpBoss HP: " + currentHP +
                " | Phase: " + currentPhase
            );
        }

        // Riddle01 solved.
        if (riddleStep == 1 &&
            riddleShield != null &&
            !riddleShield.activeSelf)
        {
            riddleStep = 2;
            shieldProtectingBoss = false;

            Debug.Log(
                "Riddle01 solved! Boss fight can begin."
            );
        }

        // Riddle02 solved.
        if (riddleStep == 3 &&
            riddleShield02 != null &&
            !riddleShield02.activeSelf)
        {
            riddleStep = 4;
            shieldProtectingBoss = false;

            Debug.Log(
                "Riddle02 solved! Boss is vulnerable again."
            );
        }
    }

    public override void TakeDamage(int amount)
    {
        if (isDefeated || currentHP <= 0)
            return;

        if (shieldProtectingBoss)
        {
            Debug.Log(
                "DumpBoss is protected! Solve the riddle first."
            );
            return;
        }

        // Stop exactly at 40 HP so Riddle02 cannot be skipped.
        if (currentPhase < 3 &&
            currentHP > maxHP * phaseThresholds[1] &&
            currentHP - amount <= maxHP * phaseThresholds[1])
        {
            amount = currentHP -
                Mathf.RoundToInt(maxHP * phaseThresholds[1]);
        }

        base.TakeDamage(amount);
    }

    protected override void OnPhaseChange(int newPhase)
    {
        base.OnPhaseChange(newPhase);

        // Tell Animator to switch phase.
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("Phase" + newPhase);
        }

        // Open Riddle02 automatically at 40 HP.
        if (newPhase == 3 && riddleStep == 2)
        {
            riddleStep = 3;
            shieldProtectingBoss = true;

            if (riddleShield02 != null)
            {
                riddleShield02.SetActive(true);

                RiddleShield secondRiddle =
                    riddleShield02.GetComponent<RiddleShield>();

                if (secondRiddle != null)
                {
                    secondRiddle.OpenRiddle(playerRb);

                    Debug.Log(
                        "40 HP: Riddle02 opened!"
                    );
                }
            }
            else
            {
                Debug.LogWarning(
                    "RiddleShield02 is missing!"
                );

                riddleStep = 4;
                shieldProtectingBoss = false;
            }
        }
    }

    protected override void OnDefeated()
    {
        if (isDefeated)
            return;

        isDefeated = true;

        base.OnDefeated();
        Die();
    }

    protected override void Die()
    {
        Debug.Log("DumpBoss defeated!");
        gameObject.SetActive(false);
    }
}
