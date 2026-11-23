
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 100;

    [Header("UI")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private HealthBar healthBar;

    [Header("Game Over")]
    [SerializeField] private GameOverManager gameOverManager;

    public int currentHP;
    public int MaxHP => maxHP;

    public UnityEvent<int> OnDamaged;
    public UnityEvent OnDied;

    private bool isDead;

    private void Awake()
    {
        currentHP = maxHP;
        isDead = false;

        UpdateHealthUI();
        UpdateHealthBar();
    }

    private void Start()
    {
        // Find GameOverManager automatisk
        if (gameOverManager == null)
        {
            gameOverManager =
                FindFirstObjectByType<GameOverManager>();
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead || amount <= 0)
            return;

        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0);

        UpdateHealthUI();
        UpdateHealthBar();

        OnDamaged?.Invoke(currentHP);

        Debug.Log("Player HP: " + currentHP);

        if (currentHP <= 0)
        {
            isDead = true;

            Debug.Log("PLAYER DIED!");

            // Behold eksisterende døds-events
            OnDied?.Invoke();

            // Vis Game Over-menuen
            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOver();
            }
            else
            {
                Debug.LogWarning(
                    "Health: GameOverManager blev ikke fundet!"
                );
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text =
                currentHP + " / " + maxHP + " HP";
        }

        // Eksisterende grøn HP-bar
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount =
                maxHP > 0
                    ? (float)currentHP / maxHP
                    : 0f;
        }
    }

    private void UpdateHealthBar()
    {
        // Eksisterende HealthBar-system
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHP, maxHP);
        }
    }
}
