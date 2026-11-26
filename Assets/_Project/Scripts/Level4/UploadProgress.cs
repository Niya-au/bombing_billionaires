
using UnityEngine;
using UnityEngine.UI;

public class UploadProgress : MonoBehaviour
{
    [Header("Upload Progress")]
    [SerializeField] private Image fillImage;
    [SerializeField] private float uploadSpeed = 0.05f;
    [SerializeField] private float gameOverThreshold = 1f;

    [Header("Ending Manager")]
    [SerializeField] private Level4EndingController endingManager;

    private float progress = 0f;
    private bool isUploading = true;
    private bool finished = false;

    private void Start()
    {
        if (fillImage != null)
            fillImage.fillAmount = 0f;

        if (endingManager == null)
        {
            endingManager =
                FindFirstObjectByType<Level4EndingController>();
        }
    }

    private void Update()
    {
        if (!isUploading || finished)
            return;

        progress += uploadSpeed * Time.deltaTime;
        progress = Mathf.Clamp01(progress);

        if (fillImage != null)
            fillImage.fillAmount = progress;

        if (progress >= gameOverThreshold)
        {
            UploadCompleted();
        }
    }

    public void SetUploadSpeed(float newSpeed)
    {
        uploadSpeed = newSpeed;

        Debug.Log("Upload speed changed to: " + uploadSpeed);
    }

    public void StopUpload()
    {
        isUploading = false;

        Debug.Log("Correct password! Upload stopped.");
    }

    private void UploadCompleted()
    {
        if (finished)
            return;

        finished = true;
        isUploading = false;

        if (fillImage != null)
            fillImage.fillAmount = 1f;

        Debug.Log("Upload completed! Mission failed.");

        if (endingManager != null)
        {
            endingManager.TriggerPlayerDeath();
        }
        else
        {
            Debug.LogError("Level4EndingController is missing!");
        }
    }
}
