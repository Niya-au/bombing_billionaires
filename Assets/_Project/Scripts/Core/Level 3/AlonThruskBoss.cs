
using UnityEngine;

public class AlonThruskBoss : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] private int maxHP = 70;
    [SerializeField] private int currentHP = 70;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveRangeX = 3f;
    [SerializeField] private float moveRangeY = 2f;

    [Header("Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackInterval = 1.5f;
    [SerializeField] private float projectileSpeed = 7f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float projectileSpawnDistance = 0.7f;

    [Header("Player")]
    [SerializeField] private Transform player;

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

        FindPlayer();

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

        GameObject playerObject =
            GameObject.Find("BK");

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


        // Find BK igen hvis nødvendigt
        if (player == null)
        {
            FindPlayer();
        }


        // ==========================================
        // ALON BEVÆGER SIG RUNDT
        // ==========================================

        MoveBoss();


        // ==========================================
        // ANGREB
        // ==========================================

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
    // ALON SHOOTS
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


        // Retning fra Alon mod BK
        Vector2 direction =
            (
                player.position -
                transform.position
            ).normalized;


        // Spawn lidt foran Alon
        Vector2 spawnPosition =
            (Vector2)transform.position +
            direction * projectileSpawnDistance;


        // Opret rødt projectile
        GameObject projectile =
            Instantiate(
                projectilePrefab,
                spawnPosition,
                Quaternion.identity
            );


        // Find Rigidbody2D
        Rigidbody2D rb =
            projectile.GetComponent<Rigidbody2D>();


        if (rb == null)
        {
            Debug.LogError(
                "🔴 ALON ERROR: Rød projectile mangler Rigidbody2D!"
            );

            Destroy(projectile);

            return;
        }


        // Send projectile mod BK
        rb.linearVelocity =
            direction * projectileSpeed;


        // Sæt damage
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
    // STOP BOSS FIGHT
    // ==========================================

    public void StopBossFight()
    {
        fightStarted = false;

        attackTimer = 0f;

        Debug.Log("ALON BOSS FIGHT STOPPED!");
    }


    // ==========================================
    // TAKE DAMAGE
    // ==========================================

    public void TakeDamage(int amount)
    {
        if (!fightStarted || bossDead)
        {
            return;
        }


        currentHP -= amount;


        if (currentHP < 0)
        {
            currentHP = 0;
        }


        Debug.Log(
            "ALON HP: " +
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
    // DIE
    // ==========================================

    private void Die()
    {
        if (bossDead)
        {
            return;
        }


        bossDead = true;
        fightStarted = false;


        Debug.Log("==============================");
        Debug.Log("ALON THRUSK DEFEATED!");
        Debug.Log("==============================");


        gameObject.SetActive(false);
    }
}
