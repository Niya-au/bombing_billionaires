
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

            OnDied?.Invoke();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text =
                currentHP + " / " + maxHP + " HP";
        }

        // Green health bar from main
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
        // Health bar from her branch
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHP, maxHP);
        }
    }
}
