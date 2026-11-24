
using UnityEngine;
using System.Collections;

public class DumpBoss : BossBase
{
    [Header("Animator")]
    [SerializeField] private Animator bossAnimator;

    [Header("Boss Visual")]
    [SerializeField] private SpriteRenderer bossSprite;
    [SerializeField] private float bulletSpawnOffset = 0.6f;
    [SerializeField] private bool spriteFacesRightByDefault = true;

    [Header("Riddle Shields")]
    [SerializeField] private GameObject riddleShield;
    [SerializeField] private GameObject riddleShield02;
    [SerializeField] private GameObject riddlePanel;

    [Header("Player")]
    [SerializeField] private Rigidbody2D playerRb;

    [Header("First Riddle Distance")]
    [SerializeField] private float riddleStartDistance = 3f;

    [Header("Security Guards")]
    [SerializeField] private SecurityGuardHealth[] securityGuards;

    [Header("Escape")]
    [SerializeField] private Transform escapeDestination;
    [SerializeField] private float escapeSpeed = 3f;

    [Header("Office Dialogue")]
    [SerializeField] private GameObject officeTwitterBox;
    [SerializeField] private float officeDialogueDuration = 4f;

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firepoint;
    [SerializeField] private float shootInterval = 2f;

    [Header("Victory")]
    [SerializeField] private DumpGameOverManager gameOverManager;
    [SerializeField] private float victoryDelay = 2f;

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
    private bool officeDialogueStarted;

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
        officeDialogueStarted = false;

        riddleStep = 1;
        shootTimer = 0f;

        if (bossAnimator == null)
            bossAnimator = GetComponent<Animator>();

        if (bossSprite == null)
            bossSprite = GetComponent<SpriteRenderer>();

        if (bossSprite == null)
            bossSprite = GetComponentInChildren<SpriteRenderer>();

        if (bossAnimator != null)
            bossAnimator.SetInteger("CurrentPhase", currentPhase);

        if (riddleShield != null)
            riddleShield.SetActive(false);

        if (riddleShield02 != null)
            riddleShield02.SetActive(false);

        if (riddlePanel != null)
            riddlePanel.SetActive(false);

        // OfficeTwitterBox skal være skjult fra starten
        if (officeTwitterBox != null)
            officeTwitterBox.SetActive(false);

        if (gameOverManager == null)
        {
            gameOverManager =
                FindFirstObjectByType<DumpGameOverManager>();
        }

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

    private void FacePlayer()
    {
        if (playerRb == null || bossSprite == null)
            return;

        bool playerIsLeft =
            playerRb.position.x < transform.position.x;

        bossSprite.flipX = spriteFacesRightByDefault
            ? playerIsLeft
            : !playerIsLeft;
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
            Debug.LogWarning(
                "Escape Destination is missing!"
            );
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

        Debug.Log("DumpBoss reached office.");
    }

    private bool AllGuardsDefeated()
    {
        if (securityGuards == null ||
            securityGuards.Length == 0)
        {
            return false;
        }

        foreach (SecurityGuardHealth guard in securityGuards)
        {
            if (guard != null &&
                guard.gameObject.activeInHierarchy)
            {
                return false;
            }
        }

        return true;
    }

    public bool AreAllGuardsDefeated()
    {
        return AllGuardsDefeated();
    }

    private bool PlayerIsNearBoss()
    {
        if (playerRb == null)
            return false;

        return Vector2.Distance(
            playerRb.position,
            transform.position
        ) <= riddleStartDistance;
    }

    private void Update()
    {
        if (isDefeated || currentHP <= 0)
            return;

        if (enableDamageTest &&
            Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(testDamage);
        }

        if (!introFinished || isEscaping || !hasEscaped)
            return;

        FacePlayer();

        // Vis OfficeTwitterBox efter alle vagter er døde
        // og BK kommer tæt på Donald Dump
        if (!officeDialogueStarted &&
            !firstRiddleStarted &&
            AllGuardsDefeated() &&
            PlayerIsNearBoss() &&
            !playerIsDead &&
            !dialogueActive)
        {
            officeDialogueStarted = true;
            StartCoroutine(PlayOfficeDialogue());
        }

        // Første gåde er løst
        if (riddleStep == 1 &&
            riddleShield != null &&
            !riddleShield.activeSelf &&
            firstRiddleStarted)
        {
            riddleStep = 2;
            shieldProtectingBoss = false;
            shootTimer = 0f;

            Debug.Log(
                "Riddle01 solved! Boss fight begins."
            );
        }

        // Anden gåde er løst
        if (riddleStep == 3 &&
            riddleShield02 != null &&
            !riddleShield02.activeSelf)
        {
            riddleStep = 4;
            shieldProtectingBoss = false;
            shootTimer = 0f;

            Debug.Log(
                "Riddle02 solved! Boss vulnerable again."
            );
        }

        // Bossen skyder ikke under dialog eller gåder
        if (!CanFight)
        {
            shootTimer = 0f;
            return;
        }

        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            Shoot();
        }
    }

    private IEnumerator PlayOfficeDialogue()
    {
        // Stop bossens skydning under dialogen
        SetDialogueActive(true);

        // Vis OfficeTwitterBox
        if (officeTwitterBox != null)
        {
            officeTwitterBox.SetActive(true);
        }
        else
        {
            Debug.LogWarning(
                "DumpBoss: OfficeTwitterBox mangler!"
            );
        }

        // Vent, mens Twitter-boksen vises
        yield return new WaitForSecondsRealtime(
            officeDialogueDuration
        );

        // Skjul Twitter-boksen
        if (officeTwitterBox != null)
        {
            officeTwitterBox.SetActive(false);
        }

        // Start første gåde
        SetDialogueActive(false);
        StartFirstRiddle();
    }

    public void StartFirstRiddle()
    {
        if (firstRiddleStarted ||
            isDefeated ||
            playerIsDead ||
            dialogueActive ||
            !introFinished ||
            isEscaping ||
            !hasEscaped ||
            !officeDialogueStarted ||
            !AllGuardsDefeated() ||
            !PlayerIsNearBoss())
        {
            return;
        }

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

        Debug.Log(
            "Office dialogue finished! Riddle01 opened."
        );
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
        if (isDefeated ||
            currentHP <= 0 ||
            !CanFight)
        {
            return;
        }

        // Stop ved fase 3, så anden gåde kan starte
        if (currentPhase < 3 &&
            currentHP > maxHP * phaseThresholds[1] &&
            currentHP - amount <=
                maxHP * phaseThresholds[1])
        {
            amount = currentHP -
                Mathf.RoundToInt(
                    maxHP * phaseThresholds[1]
                );
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
            bossAnimator.SetInteger(
                "CurrentPhase",
                newPhase
            );

            bossAnimator.SetTrigger(
                "Phase" + newPhase
            );
        }

        // Anden gåde ved fase 3
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
        // Vent på dødsanimationen
        yield return new WaitForSeconds(victoryDelay);

        // Skjul Donald Dump
        gameObject.SetActive(false);

        // Find DumpGameOverManager, hvis den ikke er sat
        if (gameOverManager == null)
        {
            gameOverManager =
                FindFirstObjectByType<DumpGameOverManager>();
        }

        // Vis VICTORY-menuen
        if (gameOverManager != null)
        {
            gameOverManager.ShowVictory();
        }
        else
        {
            Debug.LogWarning(
                "DumpBoss: DumpGameOverManager mangler!"
            );
        }
    }

    private void Shoot()
    {
        if (!CanFight ||
            projectilePrefab == null ||
            firepoint == null ||
            playerRb == null)
        {
            return;
        }

        bool playerIsLeft =
            playerRb.position.x < transform.position.x;

        FacePlayer();

        Vector3 spawnPosition = firepoint.position;

        float bossCenterX = bossSprite != null
            ? bossSprite.bounds.center.x
            : transform.position.x;

        spawnPosition.x = bossCenterX +
            (playerIsLeft
                ? -bulletSpawnOffset
                : bulletSpawnOffset);

        GameObject bullet = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.identity
        );

        DumpProjectile projectile =
            bullet.GetComponent<DumpProjectile>();

        if (projectile != null)
        {
            Vector2 direction =
                (playerRb.position -
                (Vector2)spawnPosition).normalized;

            projectile.SetDirection(direction);
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDied.RemoveListener(
                OnPlayerDied
            );
        }
    }
}
