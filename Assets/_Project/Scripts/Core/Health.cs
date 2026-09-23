using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    public int currentHP;

    public UnityEvent<int> OnDamaged;
    public UnityEvent OnDied;

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
