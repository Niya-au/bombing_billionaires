using UnityEngine;
using TMPro;

public class BKMoneyAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MoneySpawner moneySpawner;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private TMP_Text chargeText;

    [Header("Attack")]
    [SerializeField] private KeyCode fireKey = KeyCode.Space;
    [SerializeField, Min(1)] private int costPerShot = 100;
    [SerializeField] private float cooldown = 0.35f;

    private bool bossFightActive;
    private int moneyAtBossStart;
    private int moneySpent;
    private float nextShotTime;

    public void BeginBossFight()
    {
        if (bossFightActive) return;
        bossFightActive = true;
        moneyAtBossStart = moneySpawner != null ? moneySpawner.CollectedMoney : 0;
        moneySpent = 0;
        UpdateChargeText();
    }

    private void Update()
    {
        if (!bossFightActive || moneySpawner == null) return;
        if (Input.GetKeyDown(fireKey) && Time.time >= nextShotTime && Charge >= costPerShot)
        {
            if (projectilePrefab != null && firePoint != null)
            {
                Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                moneySpent += costPerShot;
                nextShotTime = Time.time + cooldown;
            }
        }
        UpdateChargeText();
    }

    private int Charge => Mathf.Max(0, moneySpawner.CollectedMoney - moneyAtBossStart - moneySpent);

    private void UpdateChargeText()
    {
        if (chargeText != null) chargeText.text = $"Attack charge: ${Charge} (Space: ${costPerShot})";
    }
}
