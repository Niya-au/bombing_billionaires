using UnityEngine;

public class EvidenceManager : MonoBehaviour
{
    public float maxEvidence = 100f;
    public float currentEvidence;
    public float decayPerSecond = 2f;

    public GameOverManager gameOverManager;

    [Header("Evidence Collection")]
    public int evidenceCollected = 0;
    public int evidenceNeeded = 5;

    [Header("Boss")]
    public GameObject boss;

    private bool gameEnded = false;

    void Start()
    {
        Time.timeScale = 1f;
        currentEvidence = maxEvidence;
        gameEnded = false;

        if (boss != null)
        {
            boss.SetActive(false);
        }
    }

    void Update()
    {
        if (gameEnded) return;

        currentEvidence -= decayPerSecond * Time.deltaTime;
        currentEvidence = Mathf.Clamp(currentEvidence, 0f, maxEvidence);

        if (currentEvidence <= 0f)
        {
            gameEnded = true;
            gameOverManager.GameOver();
        }
    }

    public void AddEvidence(float amount)
    {
        currentEvidence += amount;
        currentEvidence = Mathf.Clamp(currentEvidence, 0f, maxEvidence);

        evidenceCollected++;

        Debug.Log("Evidence collected: " + evidenceCollected + "/" + evidenceNeeded);

        if (evidenceCollected >= evidenceNeeded)
        {
            ActivateBoss();
        }
    }

    void ActivateBoss()
    {
        if (boss != null)
        {
            boss.SetActive(true);
            Debug.Log("BOSS ACTIVATED!");
        }
    }
}