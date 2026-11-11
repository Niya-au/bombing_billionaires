using UnityEngine;
using System.Collections;

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

    [Header("Security Guards")]
    [SerializeField] private SecurityGuardHealth[] securityGuards;

    [Header("Escape")]
    [SerializeField] private Transform escapeDestination;
    [SerializeField] private float escapeSpeed = 3f;

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firepoint;
    [SerializeField] private float shootInterval = 2f;

    [Header("Damage Test")]
    [SerializeField] private bool enableDamageTest = false;
    [SerializeField] private int testDamage = 10;

    private Health playerHealth;
    private bool playerIsDead;

    private bool shieldProtectingBoss;
    private bool isDefeated;
    private bool introFinished;
    private bool isEscaping;
    private bool hasEscaped;
    private bool firstRiddleStarted;
    private bool dialogueActive;

    private int riddleStep;
    private float shootTimer;

    public bool IsInteractionActive =>
        dialogueActive ||
        (riddleShield != null && riddleShield.activeSelf) ||
        (riddleShield02 != null && riddleShield02.activeSelf) ||
        (riddlePanel != null && riddlePanel.activeSelf);

    public bool CanFight =>
        firstRiddleStarted &&
        !shieldProtectingBoss &&
        !isDefeated &&
        !playerIsDead &&
        !IsInteractionActive;

    protected override void Start()
    {
        base.Start();

        isDefeated = false;
        playerIsDead = false;
        shieldProtectingBoss = true;
        introFinished = false;
        isEscaping = false;
        hasEscaped = false;
        firstRiddleStarted = false;
        dialogueActive = false;

        riddleStep = 1;
        shootTimer = 0f;

        if (bossAnimator == null)
            bossAnimator = GetComponent<Animator>();

        if (bossAnimator != null)
            bossAnimator.SetInteger("CurrentPhase", currentPhase);

        if (riddleShield != null)
            riddleShield.SetActive(false);

        if (riddleShield02 != null)
            riddleShield02.SetActive(false);

        if (riddlePanel != null)
            riddlePanel.SetActive(false);

        if (playerRb == null)
        {
            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                playerRb = player.GetComponent<Rigidbody2D>();
        }

        if (playerRb != null)
        {
            playerHealth = playerRb.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.OnDied.AddListener(OnPlayerDied);

                if (playerHealth.currentHP <= 0)
                    OnPlayerDied();
            }
        }

        Debug.Log("DumpBoss waiting for first meeting.");
    }

    public void SetDialogueActive(bool active)
    {
        dialogueActive = active;
        shootTimer = 0f;
    }

    public void FinishIntroAndEscape()
    {
        if (introFinished || isDefeated)
            return;

        introFinished = true;

        if (escapeDestination != null)
        {
            StartCoroutine(Escape());
        }
        else
        {
            Debug.LogWarning("Escape Destination is missing!");
        }
    }

    private IEnumerator Escape()
    {
        isEscaping = true;

        while (Vector2.Distance(
            transform.position,
            escapeDestination.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                escapeDestination.position,
                escapeSpeed * Time.deltaTime
            );

            yield return null;
        }

        isEscaping = false;
        hasEscaped = true;

        Debug.Log("DumpBoss reached Level 2 area.");
    }

    private bool AllGuardsDefeated()
    {
        if (securityGuards == null ||
            securityGuards.Length == 0)
            return false;

        foreach (SecurityGuardHealth guard in securityGuards)
        {
            if (guard != null && guard.gameObject.activeInHierarchy)
                return false;
        }

        return true;
    }

    private void Update()
    {
        if (isDefeated || currentHP <= 0)
            return;

        if (enableDamageTest && Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(testDamage);
        }

        if (!introFinished || isEscaping || !hasEscaped)
            return;

        if (!firstRiddleStarted && AllGuardsDefeated())
        {
            StartFirstRiddle();
        }

        if (riddleStep == 1 &&
            riddleShield != null &&
            !riddleShield.activeSelf &&
            firstRiddleStarted)
        {
            riddleStep = 2;
            shieldProtectingBoss = false;
            shootTimer = 0f;

            Debug.Log("Riddle01 solved! Boss fight begins.");
        }

        if (riddleStep == 3 &&
            riddleShield02 != null &&
            !riddleShield02.activeSelf)
        {
            riddleStep = 4;
            shieldProtectingBoss = false;
            shootTimer = 0f;

            Debug.Log("Riddle02 solved! Boss vulnerable again.");
        }

        if (CanFight)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                Shoot();
            }
        }
    }

    public void StartFirstRiddle()
    {
        if (firstRiddleStarted || isDefeated)
            return;

        firstRiddleStarted = true;
        shieldProtectingBoss = true;

        if (riddleShield != null)
        {
            riddleShield.SetActive(true);

            RiddleShield firstRiddle =
                riddleShield.GetComponent<RiddleShield>();

            if (firstRiddle != null)
                firstRiddle.OpenRiddle(playerRb);
        }

        Debug.Log("All guards defeated! Riddle01 opened.");
    }

    private void OnPlayerDied()
    {
        if (playerIsDead)
            return;

        playerIsDead = true;
        shootTimer = 0f;
    }

    public override void TakeDamage(int amount)
    {
        if (isDefeated || currentHP <= 0 || !CanFight)
            return;

        if (currentPhase < 3 &&
            currentHP > maxHP * phaseThresholds[1] &&
            currentHP - amount <= maxHP * phaseThresholds[1])
        {
            amount = currentHP -
                Mathf.RoundToInt(maxHP * phaseThresholds[1]);
        }

        int previousHP = currentHP;

        base.TakeDamage(amount);

        Debug.Log("DumpBoss HP: " + currentHP);

        if (currentHP < previousHP &&
            currentHP > 0 &&
            bossAnimator != null)
        {
            bossAnimator.SetTrigger("BossHit");
        }
    }

    protected override void OnPhaseChange(int newPhase)
    {
        base.OnPhaseChange(newPhase);

        if (bossAnimator != null)
        {
            bossAnimator.SetInteger("CurrentPhase", newPhase);
            bossAnimator.SetTrigger("Phase" + newPhase);
        }

        if (newPhase == 3 && riddleStep == 2)
        {
            riddleStep = 3;
            shieldProtectingBoss = true;
            shootTimer = 0f;

            if (riddleShield02 != null)
            {
                riddleShield02.SetActive(true);

                RiddleShield secondRiddle =
                    riddleShield02.GetComponent<RiddleShield>();

                if (secondRiddle != null)
                    secondRiddle.OpenRiddle(playerRb);
            }
            else
            {
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

        if (bossAnimator != null)
        {
            bossAnimator.ResetTrigger("BossHit");
            bossAnimator.SetTrigger("BossDefeat");
        }

        StartCoroutine(DisableAfterDefeat());
    }

    private IEnumerator DisableAfterDefeat()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void Shoot()
    {
        if (!CanFight ||
            projectilePrefab == null ||
            firepoint == null)
            return;

        GameObject bullet = Instantiate(
            projectilePrefab,
            firepoint.position,
            Quaternion.identity
        );

        DumpProjectile projectile =
            bullet.GetComponent<DumpProjectile>();

        if (projectile != null && playerRb != null)
        {
            Vector2 direction =
                (playerRb.position -
                (Vector2)firepoint.position).normalized;

            projectile.SetDirection(direction);
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnDied.RemoveListener(OnPlayerDied);
    }
}