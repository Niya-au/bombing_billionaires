using UnityEngine;

public class AlonThruskBoss : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP = 100;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveRangeX = 5f;
    [SerializeField] private float moveRangeY = 2.5f;

    [Header("Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackInterval = 1.5f;
    [SerializeField] private float projectileSpeed = 7f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float projectileSpawnDistance = 0.7f;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Health UI")]
    [SerializeField] private HealthBar healthBar;

    private float attackTimer = 0f;

    private Vector3 startPosition;

    private bool fightStarted = false;
    private bool bossDead = false;


    // ==========================================
    // AWAKE
    // ==========================================

    private void Awake()
    {
        currentHP = maxHP;

        fightStarted = false;
        bossDead = false;

        startPosition = transform.position;

        // Find healthbar på Alon
        if (healthBar == null)
        {
            healthBar = GetComponent<HealthBar>();
        }

        // Find BK
        FindPlayer();

        // Opdater healthbar
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHP, maxHP);
        }

        Debug.Log("ALON READY - WAITING FOR BOSS FIGHT");
    }


    // ==========================================
    // FIND BK
    // ==========================================

    private void FindPlayer()
    {
        if (player != null)
        {
            return;
        }

        GameObject playerObject = GameObject.Find("BK");

        if (playerObject != null)
        {
            player = playerObject.transform;

            Debug.Log("🔴 ALON FOUND BK!");
        }
    }


    // ==========================================
    // START BOSS FIGHT
    // ==========================================

    public void StartBossFight()
    {
        if (bossDead)
        {
            return;
        }

        FindPlayer();

        currentHP = maxHP;
        attackTimer = 0f;

        startPosition = transform.position;

        fightStarted = true;
        bossDead = false;

        if (healthBar != null)
        {
            healthBar.SetHealth(
                currentHP,
                maxHP
            );
        }

        Debug.Log("==============================");
        Debug.Log("ALON THRUSK BOSS FIGHT STARTED!");
        Debug.Log("ALON HP: " + currentHP + " / " + maxHP);
        Debug.Log("==============================");
    }


    // ==========================================
    // UPDATE
    // ==========================================

    private void Update()
    {
        if (!fightStarted || bossDead)
        {
            return;
        }

        if (player == null)
        {
            FindPlayer();
        }

        MoveBoss();
        AttackTimer();
    }


    // ==========================================
    // BOSS MOVEMENT
    // ==========================================

    private void MoveBoss()
    {
        float x =
            startPosition.x +
            Mathf.Sin(Time.time * moveSpeed) * moveRangeX;

        float y =
            startPosition.y +
            Mathf.Cos(Time.time * moveSpeed * 0.7f) * moveRangeY;

        transform.position =
            new Vector3(
                x,
                y,
                startPosition.z
            );
    }


    // ==========================================
    // ATTACK TIMER
    // ==========================================

    private void AttackTimer()
    {
        if (player == null)
        {
            return;
        }

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            attackTimer = 0f;

            Attack();
        }
    }


    // ==========================================
    // ALON SHOOTS RED PROJECTILE
    // ==========================================

    private void Attack()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError(
                "🔴 ALON ERROR: Projectile Prefab mangler på Alon!"
            );

            return;
        }

        if (player == null)
        {
            Debug.LogWarning(
                "🔴 ALON ERROR: BK blev ikke fundet!"
            );

            return;
        }

        Vector2 direction =
            (
                player.position -
                transform.position
            ).normalized;

        Vector2 spawnPosition =
            (Vector2)transform.position +
            direction * projectileSpawnDistance;

        GameObject projectile =
            Instantiate(
                projectilePrefab,
                spawnPosition,
                Quaternion.identity
            );

        Rigidbody2D rb =
            projectile.GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError(
                "🔴 ALON ERROR: Projectile mangler Rigidbody2D!"
            );

            Destroy(projectile);

            return;
        }

        rb.linearVelocity =
            direction * projectileSpeed;

        AlonProjectile projectileScript =
            projectile.GetComponent<AlonProjectile>();

        if (projectileScript != null)
        {
            projectileScript.SetDamage(damage);
        }
        else
        {
            Debug.LogWarning(
                "🔴 ALON WARNING: AlonProjectile script mangler!"
            );
        }

        Debug.Log("🔴 ALON ATTACKED BK!");
    }


    // ==========================================
    // TAKE DAMAGE FROM BK
    // ==========================================

    public void TakeDamage(int amount)
    {
        if (!fightStarted || bossDead)
        {
            return;
        }

        currentHP -= amount;

        currentHP =
            Mathf.Max(
                currentHP,
                0
            );

        if (healthBar != null)
        {
            healthBar.SetHealth(
                currentHP,
                maxHP
            );
        }

        Debug.Log(
            "🔵 ALON HP: " +
            currentHP +
            " / " +
            maxHP
        );

        if (currentHP <= 0)
        {
            Die();
        }
    }


    // ==========================================
    // STOP BOSS FIGHT
    // ==========================================

    public void StopBossFight()
    {
        fightStarted = false;
        attackTimer = 0f;

        Debug.Log("ALON BOSS FIGHT STOPPED!");
    }


    // ==========================================
    // DIE
    // ==========================================

    private void Die()
    {
        if (bossDead)
        {
            return;
        }

        bossDead = true;
        currentHP = 0;

        Debug.Log("==============================");
        Debug.Log("🏆 ALON THRUSK DEFEATED!");
        Debug.Log("==============================");

        // Stop Alons bevægelse og skydning
        StopBossFight();

        // Fortæl HazardSpawner at bossfighten er slut
        HazardSpawner hazardSpawner =
            FindFirstObjectByType<HazardSpawner>();

        if (hazardSpawner != null)
        {
            hazardSpawner.BossDefeated();
        }
        else
        {
            Debug.LogWarning(
                "ALON: Kunne ikke finde HazardSpawner!"
            );
        }

        // Skjul Alon
        gameObject.SetActive(false);
    }
}