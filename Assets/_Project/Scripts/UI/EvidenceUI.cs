using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvidenceUI : MonoBehaviour
{
    public EvidenceManager evidenceManager;
    public Slider evidenceSlider;
    public TMP_Text evidenceText;

    void Update()
    {
        evidenceSlider.value = evidenceManager.currentEvidence;

        evidenceText.text =
            "Evidence: " +
            Mathf.RoundToInt(evidenceManager.currentEvidence) +
            "%";
    }
}