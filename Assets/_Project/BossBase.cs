using UnityEngine;

public abstract class BossBase : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected int maxHP = 100;
    protected int currentHP;

    [Header("Boss Phase")]
    [SerializeField]
    protected float[] phaseThresholds = { 0.7f, 0.4f };

    protected int currentPhase = 1;

    protected virtual void Start()
    {
        currentHP = maxHP;
        UpdatePhase();
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        UpdatePhase();

        if (currentHP <= 0)
        {
            Die();
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

    protected abstract void Die();
}