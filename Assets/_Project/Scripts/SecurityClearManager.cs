
using UnityEngine;
using System.Collections;

public class SecurityClearManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject securityClearedPanel;

    private int guardsRemaining;
    private bool messageShown;

    private void Start()
    {
        // Tæl alle vagter i scenen
        guardsRemaining =
            FindObjectsByType<SecurityGuardHealth>(
                FindObjectsSortMode.None
            ).Length;

        if (securityClearedPanel != null)
        {
            securityClearedPanel.SetActive(false);
        }

        Debug.Log("Security Guards remaining: " + guardsRemaining);
    }

    public void GuardDefeated()
    {
        if (messageShown)
            return;

        guardsRemaining--;

        Debug.Log("Security Guards remaining: " + guardsRemaining);

        if (guardsRemaining <= 0)
        {
            messageShown = true;
            StartCoroutine(ShowSecurityCleared());
        }
    }

    private IEnumerator ShowSecurityCleared()
    {
        if (securityClearedPanel != null)
        {
            securityClearedPanel.SetActive(true);
        }

        yield return new WaitForSeconds(3f);

        if (securityClearedPanel != null)
        {
            securityClearedPanel.SetActive(false);
        }
    }
}
