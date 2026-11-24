using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private Image fillImage;

    private void Update()
    {
        fillImage.fillAmount =
            (float)enemyHealth.CurrentHealth / enemyHealth.MaxHealth;
    }
}