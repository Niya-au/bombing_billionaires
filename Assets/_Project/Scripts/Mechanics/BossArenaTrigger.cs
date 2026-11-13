using UnityEngine;

public class BossArenaTrigger : MonoBehaviour
{
    public GameObject bossGate;
    public EvidenceManager evidenceManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (evidenceManager.evidenceCollected >= evidenceManager.evidenceNeeded)
        {
            bossGate.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}