
using UnityEngine;

public class GuardBullet : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 5;
    [SerializeField] private float lifetime = 4f;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer bulletSprite;
    [SerializeField] private bool spriteFacesLeftByDefault = false;

    private bool hasHit;

    private void Awake()
    {
        if (bulletSprite == null)
            bulletSprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 direction)
    {
        if (bulletSprite == null ||
            Mathf.Abs(direction.x) < 0.001f)
            return;

        bool shootingLeft = direction.x < 0f;

        bulletSprite.flipX = spriteFacesLeftByDefault
            ? !shootingLeft
            : shootingLeft;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        // Find BK's PlayerController
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        // Find BK's Health
        Health playerHealth =
            player.GetComponent<Health>();

        if (playerHealth == null)
            playerHealth =
                player.GetComponentInChildren<Health>();

        if (playerHealth == null)
        {
            Debug.LogWarning(
                "GuardBullet ramte BK, men kunne ikke finde Health!"
            );
            return;
        }

        // Giv BK skade
        hasHit = true;
        playerHealth.TakeDamage(damage);

        Debug.Log("Security Guard ramte BK! Damage: " + damage);

        Destroy(gameObject);
    }
}
