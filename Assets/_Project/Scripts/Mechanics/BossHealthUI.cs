using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Health bossHealth;
    public Slider bossSlider;

    void Start()
    {
        bossSlider.maxValue = bossHealth.MaxHP;
        bossSlider.value = bossHealth.currentHP;
    }

    void Update()
    {
        bossSlider.value = bossHealth.currentHP;
    }
}