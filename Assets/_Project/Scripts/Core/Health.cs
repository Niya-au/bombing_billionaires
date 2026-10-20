using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    public int currentHP;
    public int MaxHP => maxHP;

    public UnityEvent<int> OnDamaged = new UnityEvent<int>();
    public UnityEvent OnDied = new UnityEvent();

    private bool isDead;

    void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0);

        OnDamaged?.Invoke(currentHP);

        if (currentHP == 0)
        {
            isDead = true;
            OnDied?.Invoke();
        }
    }
}