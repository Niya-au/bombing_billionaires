
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

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firepoint;
    [SerializeField] private float shootInterval = 2f;

    private Health playerHealth;
    private bool playerIsDead;

    private bool shieldProtectingBoss;
    private bool isDefeated;
    private int riddleStep;
    private float shootTimer;

    protected override void Start()
    {
        base.Start();

        isDefeated = false;
        playerIsDead = false;
        riddleStep = 1;
        shieldProtectingBoss = true;
        shootTimer = 0f;

        if (bossAnimator == null)
            bossAnimator = GetComponent<Animator>();

        if (bossAnimator != null)
            bossAnimator.SetInteger("CurrentPhase", currentPhase);

        if (riddleShield02 != null)
            riddleShield02.SetActive(false);

        if (riddlePanel != null)
            riddlePanel.SetActive(false);

        // Find Player if needed.
        if (playerRb == null)
        {
            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                playerRb = player.GetComponent<Rigidbody2D>();
        }

        // Find the player's Health component.
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

        // Open Riddle01 at 100 HP.
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

        // Test boss damage with H.
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
            shootTimer = 0f;

            Debug.Log("Riddle01 solved! Boss fight can begin.");
        }

        // Riddle02 solved.
        if (riddleStep == 3 &&
            riddleShield02 != null &&
            !riddleShield02.activeSelf)
        {
            riddleStep = 4;
            shieldProtectingBoss = false;
            shootTimer = 0f;

            Debug.Log("Riddle02 solved! Boss is vulnerable again.");
        }

        // Stop shooting if Tyrone is dead.
        if (playerIsDead)
            return;

        // Shoot only when boss is vulnerable.
        if (!shieldProtectingBoss)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                Shoot();
            }
        }
    }

    private void OnPlayerDied()
    {
        if (playerIsDead)
            return;

        playerIsDead = true;
        shootTimer = 0f;

        Debug.Log("Tyrone died! DumpBoss stopped shooting.");
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

        // Stop exactly at 40 HP.
        if (currentPhase < 3 &&
            currentHP > maxHP * phaseThresholds[1] &&
            currentHP - amount <= maxHP * phaseThresholds[1])
        {
            amount = currentHP -
                Mathf.RoundToInt(maxHP * phaseThresholds[1]);
        }

        int previousHP = currentHP;

        base.TakeDamage(amount);

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

        // Open Riddle02 at 40 HP.
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
                {
                    secondRiddle.OpenRiddle(playerRb);

                    Debug.Log("40 HP: Riddle02 opened!");
                }
            }
            else
            {
                Debug.LogWarning("RiddleShield02 is missing!");

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

    private System.Collections.IEnumerator DisableAfterDefeat()
    {
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
    }

    private void Shoot()
    {
        if (playerIsDead ||
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

        Debug.Log("DumpBoss fired a projectile!");
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnDied.RemoveListener(OnPlayerDied);
    }
}
