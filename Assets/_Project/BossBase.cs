using UnityEngine;
using UnityEngine.Events;

public abstract class BossBase : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected int maxHP = 100;
    protected int currentHP;

    [Header("Boss Phase")]
    [SerializeField]
    protected float[] phaseThresholds = { 0.7f, 0.4f };

    protected int currentPhase = 1;

    // Event called when any boss is defeated
    public static UnityEvent<string> OnAnyBossDefeated =
        new UnityEvent<string>();

    protected virtual void Start()
    {
        currentHP = maxHP;
        UpdatePhase();
    }

    public virtual void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        UpdatePhase();

        if (currentHP <= 0)
        {
            OnDefeated();
        }
    }

    protected virtual void UpdatePhase()
    {
        int oldPhase = currentPhase;

        float healthPercentage = (float)currentHP / maxHP;

        if (healthPercentage <= phaseThresholds[1])
        {
            currentPhase = 3;
        }
        else if (healthPercentage <= phaseThresholds[0])
        {
            currentPhase = 2;
        }
        else
        {
            currentPhase = 1;
        }

        if (currentPhase != oldPhase)
        {
            OnPhaseChange(currentPhase);
        }
    }

    protected virtual void OnPhaseChange(int newPhase)
    {
        Debug.Log("Boss changed to phase " + newPhase);
    }

    protected virtual void OnDefeated()
    {
        Debug.Log("Boss defeated: " + gameObject.name);

        OnAnyBossDefeated.Invoke(gameObject.name);
    }

    protected abstract void Die();
}