
using UnityEngine;
using UnityEngine.UI;

public class AlonHealthBarUI : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] private BossBase alonBoss;

    [Header("Health Bar")]
    [SerializeField] private Image fillImage;
    [SerializeField] private float smoothSpeed = 5f;

    private float targetFillAmount;

    private void Start()
    {
        if (alonBoss == null || fillImage == null)
        {
            Debug.LogError(
                "AlonHealthBarUI: Missing boss or fill image!"
            );

            enabled = false;
            return;
        }

        targetFillAmount = 1f;
        fillImage.fillAmount = 1f;
    }

    private void Update()
    {
        if (alonBoss == null)
            return;

        // Calculate Alon's remaining health.
        targetFillAmount = Mathf.Clamp01(
            (float)alonBoss.CurrentHP / alonBoss.MaxHP
        );

        // Smoothly animate the health bar.
        fillImage.fillAmount = Mathf.Lerp(
            fillImage.fillAmount,
            targetFillAmount,
            smoothSpeed * Time.deltaTime
        );
    }
}
