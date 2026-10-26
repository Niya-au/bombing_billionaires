using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;

    [Header("UI")]
    [SerializeField] private TMP_Text healthText;

    public int currentHP;
    public int MaxHP => maxHP;

    public UnityEvent<int> OnDamaged;
    public UnityEvent OnDied;

    private bool isDead;

    private void Awake()
    {
        currentHP = maxHP;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0);

        Debug.Log("Player HP: " + currentHP);

        UpdateHealthUI();

        OnDamaged?.Invoke(currentHP);

        if (currentHP == 0)
        {
            isDead = true;
            OnDied?.Invoke();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHP + " / " + maxHP;
        }
    }
}