using UnityEngine;
using UnityEngine.UI;

public class MoneyBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider slider;

    [Header("Animation")]
    [SerializeField] private float barSpeed = 500f;

    private float targetValue;
    private float maxValue = 1000f;


    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = maxValue;
            slider.value = 0f;
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


    public void SetMoney(int money, int goal)
    {
        if (slider == null)
            return;

        maxValue = goal;

        slider.minValue = 0f;
        slider.maxValue = maxValue;

        targetValue = Mathf.Clamp(
            money,
            0,
            maxValue
        );
    }
}