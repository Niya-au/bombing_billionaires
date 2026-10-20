using UnityEngine;

public class EvidencePickup : MonoBehaviour
{
    public float evidenceAmount = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EvidenceManager evidenceManager =
                FindFirstObjectByType<EvidenceManager>();

            if (evidenceManager != null)
            {
                evidenceManager.AddEvidence(evidenceAmount);
            }

            Destroy(gameObject);
        }
    }
}