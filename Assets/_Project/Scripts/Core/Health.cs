
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 100;

    [Header("UI")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private HealthBar healthBar;

    public int currentHP;
    public int MaxHP => maxHP;

    public UnityEvent<int> OnDamaged;
    public UnityEvent OnDied;

    private bool isDead;


    private void Awake()
    {
        currentHP = maxHP;

        UpdateHealthUI();
        UpdateHealthBar();
    }


    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        if (amount <= 0)
            return;

        currentHP -= amount;

        currentHP = Mathf.Max(
            currentHP,
            0
        );

        UpdateHealthUI();
        UpdateHealthBar();

        OnDamaged?.Invoke(currentHP);

        Debug.Log(
            "Player HP: " +
            currentHP
        );


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
                "HP: " +
                currentHP +
                " / " +
                maxHP;
        }
    }


    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(
                currentHP,
                maxHP
            );
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }
    }
}
