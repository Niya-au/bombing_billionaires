
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider slider;

    [Header("Animation")]
    [SerializeField] private float barSpeed = 300f;

    private float targetValue;

    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 100f;
            slider.value = 100f;
            targetValue = 100f;
        }
    }

    private void Update()
    {
        if (slider == null)
            return;

        slider.value = Mathf.MoveTowards(
            slider.value,
            targetValue,
            barSpeed * Time.deltaTime
        );
    }

    public void SetHealth(int currentHealth, int maxHealth)
    {
        if (slider == null)
            return;

        slider.minValue = 0f;
        slider.maxValue = maxHealth;

        targetValue = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth
        );
    }
}
