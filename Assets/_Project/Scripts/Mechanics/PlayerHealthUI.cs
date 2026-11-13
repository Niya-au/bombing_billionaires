using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Health playerHealth;
    public Slider healthSlider;

    void Start()
    {
        healthSlider.maxValue = playerHealth.MaxHP;
        healthSlider.value = playerHealth.currentHP;
    }

    void Update()
    {
        healthSlider.value = playerHealth.currentHP;
    }
}