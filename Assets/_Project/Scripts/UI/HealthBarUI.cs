using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image fillImage;
    [SerializeField] private float smoothSpeed = 5f;

    private float targetFillAmount;

    private void Start()
    {
        playerHealth.OnDamaged.AddListener(UpdateHealthBar);

        targetFillAmount = (float)playerHealth.currentHP / playerHealth.MaxHP;
        fillImage.fillAmount = targetFillAmount;
    }

    private void Update()
    {
        fillImage.fillAmount = Mathf.Lerp(
            fillImage.fillAmount,
            targetFillAmount,
            smoothSpeed * Time.deltaTime
        );
    }

    private void UpdateHealthBar(int currentHP)
    {
        targetFillAmount = (float)currentHP / playerHealth.MaxHP;
    }

    private void OnDestroy()
    {
        playerHealth.OnDamaged.RemoveListener(UpdateHealthBar);
    }
}