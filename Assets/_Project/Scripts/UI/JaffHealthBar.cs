
using UnityEngine;
using UnityEngine.UI;

public class JaffHealthBar : MonoBehaviour
{
    [Header("Jaff Health")]
    [SerializeField] private BossBase jaff;

    [Header("Health Bar UI")]
    [SerializeField] private Image fillImage;
    [SerializeField] private float smoothSpeed = 5f;

    private float targetFillAmount;

    private void Start()
    {
        if (jaff == null || fillImage == null)
        {
            Debug.LogWarning("Jaff Health Bar references are missing!");
            enabled = false;
            return;
        }

        targetFillAmount = GetHealthPercentage();
        fillImage.fillAmount = targetFillAmount;
    }

    private void Update()
    {
        if (jaff == null)
            return;

        // Get Jaff's current health percentage.
        targetFillAmount = GetHealthPercentage();

        // Smoothly animate the health bar.
        fillImage.fillAmount = Mathf.Lerp(
            fillImage.fillAmount,
            targetFillAmount,
            Mathf.Clamp01(smoothSpeed * Time.deltaTime)
        );
    }

    private float GetHealthPercentage()
    {
        if (jaff.MaxHP <= 0)
            return 0f;

        return Mathf.Clamp01(
            (float)jaff.CurrentHP / jaff.MaxHP
        );
    }
}
