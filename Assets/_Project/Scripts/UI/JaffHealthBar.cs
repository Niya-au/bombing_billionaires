
using UnityEngine;
using UnityEngine.UI;

public class JaffHealthBar : MonoBehaviour
{
    public BossBase jaff;
    public Slider healthSlider;

    void Start()
    {
        if (jaff == null || healthSlider == null)
            return;

        healthSlider.minValue = 0;
        healthSlider.maxValue = jaff.MaxHP;
        healthSlider.value = jaff.CurrentHP;
    }

    void Update()
    {
        if (jaff == null || healthSlider == null)
            return;

        healthSlider.value = jaff.CurrentHP;
    }
}

