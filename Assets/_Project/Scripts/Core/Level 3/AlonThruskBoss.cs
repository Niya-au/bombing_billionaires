using UnityEngine;

public class AlonThruskBoss : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] private int maxHP = 200;
    [SerializeField] private int currentHP = 200;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveRange = 2f;

    [Header("Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackInterval = 1.5f;
    [SerializeField] private float projectileSpeed = 7f;
    [SerializeField] private int damage = 10;

    [Header("Player")]
    [SerializeField] private Transform player;

    private float attackTimer = 0f;
    private float startY = 0f;

    private bool fightStarted = false;
    private bool bossDead = false;


    private void Awake()
    {
        currentHP = maxHP;
        fightStarted = false;
        bossDead = false;

        // Find BK automatisk
        if (player == null)
        {
            GameObject playerObject = GameObject.Find("BK");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        Debug.Log("ALON READY - WAITING FOR BOSS FIGHT");
    }


    // Denne funktion bliver kaldt af HazardSpawner
    // efter de 15 sekunders hazard-fase
    public void StartBossFight()
    {
        if (bossDead)
        {
            return;
        }

        // Find BK igen hvis nødvendigt
        if (player == null)
        {
            GameObject playerObject = GameObject.Find("BK");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        currentHP = maxHP;
        attackTimer = 0f;

        startY = transform.position.y;

        fightStarted = true;
        bossDead = false;

        Debug.Log("==============================");
        Debug.Log("ALON THRUSK BOSS FIGHT STARTED!");
        Debug.Log("ALON HP: " + currentHP + " / " + maxHP);
        Debug.Log("==============================");
    }


    private void Update()
    {
        // Gør ingenting før HazardSpawner starter boss fighten
        if (!fightStarted || bossDead)
        {
            return;
        }


        // ==============================
        // ALON BEVÆGER SIG OP OG NED
        // ==============================

        float y =
            startY +
            Mathf.Sin(Time.time * moveSpeed) * moveRange;

        transform.position = new Vector3(
            transform.position.x,
            y,
            transform.position.z
        );


        // ==============================
        // ANGREBSTIMER
        // ==============================

        attackTimer += Time.deltaTime;


        if (attackTimer >= attackInterval)
        {
            Attack();

            attackTimer = 0f;
        }
    }


    private void Attack()
    {
        // Mangler projectile
        if (projectilePrefab == null)
        {
            Debug.LogWarning(
                "ALON WARNING: Projectile Prefab mangler!"
            );

            return;
        }


        // Mangler BK
        if (player == null)
        {
            Debug.LogWarning(
                "ALON WARNING: BK blev ikke fundet!"
            );

            return;
        }


        // Spawn projectile
        GameObject projectile = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );


        // Find Rigidbody2D
        Rigidbody2D rb =
            projectile.GetComponent<Rigidbody2D>();


        if (rb != null)
        {
            Vector2 direction =
                (player.position - transform.position).normalized;

            rb.linearVelocity =
                direction * projectileSpeed;
        }
        else
        {
            Debug.LogWarning(
                "ALON WARNING: Projectile mangler Rigidbody2D!"
            );
        }


        // Find projectile-script
        AlonProjectile projectileScript =
            projectile.GetComponent<AlonProjectile>();


        if (projectileScript != null)
        {
            projectileScript.SetDamage(damage);
        }
        else
        {
            Debug.LogWarning(
                "ALON WARNING: AlonProjectile script mangler på projectile!"
            );
        }


        Debug.Log("ALON ATTACKED BK!");
    }


    public void TakeDamage(int amount)
    {
        // Kan ikke tage skade før boss fighten
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


        // Stop bossens angreb
        CancelInvoke();


        // Skjul Alon
        gameObject.SetActive(false);
    }
}