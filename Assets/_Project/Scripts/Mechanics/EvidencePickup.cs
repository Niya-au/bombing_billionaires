using UnityEngine;

public class EvidencePickup : MonoBehaviour
{
    public float evidenceAmount = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EvidenceManager manager = FindFirstObjectByType<EvidenceManager>();

            if (manager != null)
            {
                manager.AddEvidence(evidenceAmount);
            }

            Destroy(gameObject);
        }
    }
}