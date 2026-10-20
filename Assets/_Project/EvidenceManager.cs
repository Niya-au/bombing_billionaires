using UnityEngine;

public class EvidenceManager : MonoBehaviour
{
    public float maxEvidence = 100f;
    public float currentEvidence = 100f;

    public float decayPerSecond = 2f;

    void Update()
    {
        // Evidence falder over tid
        currentEvidence -= decayPerSecond * Time.deltaTime;

        // Sørger for at værdien bliver mellem 0 og max
        currentEvidence = Mathf.Clamp(currentEvidence, 0f, maxEvidence);

        if (currentEvidence <= 0)
        {
            Debug.Log("Evidence lost!");
        }
    }

    public void AddEvidence(float amount)
    {
        currentEvidence += amount;
        currentEvidence = Mathf.Clamp(currentEvidence, 0f, maxEvidence);
    }

    public float GetEvidencePercent()
    {
        return currentEvidence / maxEvidence;
    }
}