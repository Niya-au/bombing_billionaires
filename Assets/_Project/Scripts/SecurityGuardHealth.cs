
using UnityEngine;

public class SecurityGuardHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 30;

    private int currentHP;
    private bool isDead;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        if (isDead || amount <= 0)
            return;

        currentHP = Mathf.Max(0, currentHP - amount);

        Debug.Log("Security Guard HP: " + currentHP);

        if (currentHP == 0)
        {
            isDead = true;
            Debug.Log("Security Guard defeated!");
            Destroy(gameObject);
        }
    }
}
