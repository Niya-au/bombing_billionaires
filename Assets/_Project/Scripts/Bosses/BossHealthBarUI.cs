
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] private BossBase boss;

    [Header("Health Bar")]
    [SerializeField] private Image fillImage;
    [SerializeField] private float smoothSpeed = 5f;

    private void Start()
    {
        if (boss == null || fillImage == null)
        {
            Debug.LogError("BossHealthBarUI: Missing references!");
            enabled = false;
            return;
        }

        fillImage.fillAmount = 1f;
    }

    private void Update()
    {
        if (boss == null || boss.MaxHP <= 0)
            return;

        float targetFillAmount = Mathf.Clamp01(
            (float)boss.CurrentHP / boss.MaxHP
        );

        fillImage.fillAmount = Mathf.Lerp(
            fillImage.fillAmount,
            targetFillAmount,
            smoothSpeed * Time.unscaledDeltaTime
        );

        if (Mathf.Abs(fillImage.fillAmount - targetFillAmount) < 0.001f)
        {
            fillImage.fillAmount = targetFillAmount;
        }
    }
}
