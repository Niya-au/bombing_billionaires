
using UnityEngine;

public class MissionEndTrigger : MonoBehaviour
{
    public LevelEndingController endingController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (endingController != null)
            endingController.TriggerMissionSuccess();
    }
}

