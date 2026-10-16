
using UnityEngine;

public class DumpBoss : BossBase
{
    [Header("Attack Settings")]
    [SerializeField] private float phase1AttackInterval = 3f;
    [SerializeField] private float phase2AttackInterval = 1.5f;
    [SerializeField] private float phase3AttackInterval = 1f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform player;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject riddleShield;
    [SerializeField] private GameObject riddlePanel;

    [Header("Testing Only")]
    [SerializeField] private bool enableDamageTest = true;
    [SerializeField] private int testDamage = 10;

    private float attackTimer;
    private float currentAttackInterval;
    private bool isDefeated;
    private bool shieldProtectingBoss;

    protected override void Start()
    {
        base.Start();

        currentAttackInterval = phase1AttackInterval;

        if (riddleShield != null)
            riddleShield.SetActive(false);

        Debug.Log("DumpBoss started with " + currentHP + " HP");
    }

    private void Update()
    {
        if (isDefeated || currentHP <= 0)
            return;

        // Temporary keyboard test for boss damage.
        if (enableDamageTest && Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(testDamage);

            Debug.Log(
                "DumpBoss HP: " + currentHP +
                " | Phase: " + currentPhase
            );
        }

        // Check if the riddle shield has been removed.
        if (shieldProtectingBoss &&
            riddleShield != null &&
            !riddleShield.activeSelf)
        {
            shieldProtectingBoss = false;
            Debug.Log("Riddle solved! DumpBoss is vulnerable again.");
        }

        // Pause boss attacks while answering a riddle.
        if (riddlePanel != null && riddlePanel.activeInHierarchy)
            return;

        attackTimer += Time.deltaTime;

        if (attackTimer >= currentAttackInterval)
        {
            attackTimer = 0f;
            Attack();
        }
    }

    public override void TakeDamage(int amount)
    {
        if (isDefeated || currentHP <= 0)
            return;

        if (shieldProtectingBoss)
        {
            Debug.Log("DumpBoss is protected by the riddle shield!");
            return;
        }

        // Stop at 40 HP when entering phase 3.
        if (currentPhase < 3 &&
            currentHP > maxHP * phaseThresholds[1] &&
            currentHP - amount <= maxHP * phaseThresholds[1])
        {
            amount = currentHP -
                Mathf.RoundToInt(maxHP * phaseThresholds[1]);
        }

        base.TakeDamage(amount);
    }

    private void Attack()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning(
                "DumpBoss needs a Projectile Prefab and Fire Point."
            );
            return;
        }

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        DumpProjectile projectile =
            projectileObject.GetComponent<DumpProjectile>();

        if (projectile != null && player != null)
        {
            Vector2 direction =
                player.position - firePoint.position;

            projectile.SetDirection(direction);
        }

        Debug.Log("DumpBoss attacks in phase " + currentPhase);
    }

    protected override void OnPhaseChange(int newPhase)
    {
        base.OnPhaseChange(newPhase);

        switch (newPhase)
        {
            case 1:
                currentAttackInterval = phase1AttackInterval;
                break;

            case 2:
                currentAttackInterval = phase2AttackInterval;
                break;

            case 3:
                currentAttackInterval = phase3AttackInterval;

                if (riddleShield != null)
                {
                    riddleShield.SetActive(true);
                    shieldProtectingBoss = true;
                    Debug.Log("Phase 3: Riddle shield activated!");
                }
                else
                {
                    Debug.LogWarning(
                        "Phase 3 has no riddle shield assigned."
                    );
                }

                break;
        }

        attackTimer = 0f;

        if (animator != null)
            animator.SetTrigger("Phase" + newPhase);
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
        Debug.Log("DumpBoss died!");
        Destroy(gameObject);
    }
}
