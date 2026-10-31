
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 100;

    [Header("UI")]
    [SerializeField] private TMP_Text healthText;

    public int currentHP;
    public int MaxHP => maxHP;

    public UnityEvent<int> OnDamaged = new UnityEvent<int>();
    public UnityEvent OnDied = new UnityEvent();

    private bool isDead;


    private void Awake()
    {
        currentHP = maxHP;

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHP;
        }
    }


    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        currentHP -= amount;

        currentHP = Mathf.Max(currentHP, 0);

        // Opdater teksten på skærmen
        UpdateHealthUI();

        // Send event videre hvis noget bruger den
        OnDamaged?.Invoke(currentHP);

        Debug.Log("Player HP: " + currentHP);


        // Spilleren døde
        if (currentHP <= 0)
        {
            isDead = true;

            Debug.Log("PLAYER DIED!");

            OnDied?.Invoke();
        }
    }
}
