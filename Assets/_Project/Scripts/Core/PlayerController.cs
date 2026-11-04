using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.4f, 0.1f);
    [SerializeField] private Vector2 groundCheckOffset = new Vector2(0f, -0.5f);

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float shootInterval = 0.8f;
    [SerializeField] private float projectileSpawnDistance = 0.6f;

    private Rigidbody2D rb;
    private VineSwing vineSwing;

    private bool isGrounded;

    private Transform boss;
    private float shootTimer = 0f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vineSwing = GetComponent<VineSwing>();

        FindBoss();

        Debug.Log("🔵 BK SHOOTING SYSTEM READY");
    }


    void Update()
    {
        // Find Alon hvis han endnu ikke eksisterer
        if (boss == null)
        {
            FindBoss();
        }


        // ==========================================
        // AUTOMATISK SKYDNING MOD ALON
        // ==========================================

        if (boss != null && boss.gameObject.activeInHierarchy)
        {
            shootTimer -= Time.deltaTime;

            if (shootTimer <= 0f)
            {
                ShootAtBoss();
                shootTimer = shootInterval;
            }
        }


        // ==========================================
        // VINE SWING
        // ==========================================

        if (vineSwing != null && vineSwing.IsSwinging())
        {
            return;
        }


        // ==========================================
        // GROUND CHECK
        // ==========================================

        Vector2 checkPos =
            (Vector2)transform.position + groundCheckOffset;

        isGrounded =
            Physics2D.OverlapBox(
                checkPos,
                groundCheckSize,
                0f,
                groundLayer
            );


        // ==========================================
        // LEFT
        // ==========================================

        if (
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.A)
        )
        {
            rb.linearVelocity =
                new Vector2(
                    -moveSpeed,
                    rb.linearVelocity.y
                );
        }


        // ==========================================
        // RIGHT
        // ==========================================

        else if (
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.D)
        )
        {
            rb.linearVelocity =
                new Vector2(
                    moveSpeed,
                    rb.linearVelocity.y
                );
        }


        // ==========================================
        // STOP
        // ==========================================

        else if (isGrounded)
        {
            rb.linearVelocity =
                new Vector2(
                    0f,
                    rb.linearVelocity.y
                );
        }


        // ==========================================
        // JUMP
        // ==========================================

        bool jumpPressed =
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.Space);

        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity =
                new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );
        }
    }


    // ==========================================
    // FIND ALON
    // ==========================================

    private void FindBoss()
    {
        GameObject bossObject =
            GameObject.Find("alon_0");

        if (bossObject != null)
        {
            boss = bossObject.transform;

            Debug.Log("🔵 BK FOUND ALON!");
        }
    }


    // ==========================================
    // SHOOT AT ALON
    // ==========================================

    private void ShootAtBoss()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError(
                "🔵 BK ERROR: Projectile Prefab mangler!"
            );

            return;
        }


        if (boss == null)
        {
            return;
        }


        Vector2 direction =
            (
                boss.position -
                transform.position
            ).normalized;


        // Spawn lidt foran BK
        Vector2 spawnPosition =
            (Vector2)transform.position +
            direction * projectileSpawnDistance;


        GameObject projectile =
            Instantiate(
                projectilePrefab,
                spawnPosition,
                Quaternion.identity
            );


        Rigidbody2D projectileRb =
            projectile.GetComponent<Rigidbody2D>();


        if (projectileRb == null)
        {
            Debug.LogError(
                "🔵 BK ERROR: Blå projectile mangler Rigidbody2D!"
            );

            Destroy(projectile);
            return;
        }


        projectileRb.linearVelocity =
            direction * projectileSpeed;


        Debug.Log("🔵 BK SHOT ALON!");
    }
}