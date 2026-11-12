using UnityEngine;
using UnityEngine.UI;

public class UploadProgress : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Health playerHealth;

    [SerializeField] private float uploadSpeed = 0.05f;

    private float progress = 0f;
    private bool isUploading = true;
    private bool finished = false;

    private void Start()
    {
        fillImage.fillAmount = 0f;
    }

    private void Update()
    {
        if (!isUploading || finished)
            return;

        progress += uploadSpeed * Time.deltaTime;

        progress = Mathf.Clamp01(progress);

        fillImage.fillAmount = progress;

        if (progress >= 1f)
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

        Debug.Log("Upload reached 100%! Player dies.");

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(playerHealth.MaxHP);
        }
    }
}