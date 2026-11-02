using UnityEngine;

public class EvidenceManager : MonoBehaviour
{
    public float maxEvidence = 100f;
    public float currentEvidence = 100f;
    public float decayPerSecond = 2f;

    void Update()
    {
        currentEvidence -= decayPerSecond * Time.deltaTime;
        currentEvidence = Mathf.Clamp(currentEvidence, 0f, maxEvidence);
    }

    public void AddEvidence(float amount)
    {
        currentEvidence += amount;
        currentEvidence = Mathf.Clamp(currentEvidence, 0f, maxEvidence);
    }
}