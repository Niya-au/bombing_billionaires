
using UnityEngine;

public class BossUIController : MonoBehaviour
{
    [Header("Money UI")]
    [SerializeField] private GameObject moneyBar;
    [SerializeField] private GameObject moneyIcon;

    [Header("Boss Health Bars")]
    [SerializeField] private GameObject bkHealthBar;
    [SerializeField] private GameObject alonHealthBar;

    private void Awake()
    {
        // Boss bars skjules i starten
        if (bkHealthBar != null)
            bkHealthBar.SetActive(false);

        if (alonHealthBar != null)
            alonHealthBar.SetActive(false);

        // Money UI vises i starten
        if (moneyBar != null)
            moneyBar.SetActive(true);

        if (moneyIcon != null)
            moneyIcon.SetActive(true);
    }

    public void StartBossUI()
    {
        Debug.Log("BOSS UI STARTED!");

        // Skjul money UI
        if (moneyBar != null)
            moneyBar.SetActive(false);

        if (moneyIcon != null)
            moneyIcon.SetActive(false);

        // Vis boss bars
        if (bkHealthBar != null)
            bkHealthBar.SetActive(true);

        if (alonHealthBar != null)
            alonHealthBar.SetActive(true);
    }
}
