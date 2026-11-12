using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PasswordInputUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text attemptsText;
    [SerializeField] private UploadProgress uploadProgress;

    [SerializeField] private string correctPassword = "MARP5";
    [SerializeField] private int maxAttempts = 3;

    [SerializeField] private float dataUploadSpeed = 0.02f;

    public UnityEvent<float> OnDataUploadSpeedChanged;
    public UnityEvent OnPasswordCorrect;
    public UnityEvent OnOutOfAttempts;

    private int attemptsRemaining;
    private int wrongAttempts = 0;

    private void Start()
    {
        attemptsRemaining = maxAttempts;
        UpdateAttemptsText();

        if (uploadProgress != null)
        {
            uploadProgress.SetUploadSpeed(dataUploadSpeed);
        }
    }

    public void SubmitPassword()
    {
        if (attemptsRemaining <= 0)
        {
            return;
        }

        string attempt = passwordInput.text.Trim();

        // KORREKT PASSWORD
        if (attempt.Equals(
            correctPassword,
            System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Correct password!");

            if (uploadProgress != null)
            {
                uploadProgress.StopUpload();
            }

            OnPasswordCorrect?.Invoke();

            passwordInput.interactable = false;
            return;
        }

        // FORKERT PASSWORD
        attemptsRemaining--;
        attemptsRemaining = Mathf.Max(attemptsRemaining, 0);

        wrongAttempts++;

        // Bestem upload speed efter antal forkerte forsøg
        if (wrongAttempts == 1)
        {
            dataUploadSpeed = 0.03f;
        }
        else if (wrongAttempts == 2)
        {
            dataUploadSpeed = 0.04f;
        }
        else if (wrongAttempts >= 3)
        {
            dataUploadSpeed = 0.06f;
        }

        if (uploadProgress != null)
        {
            uploadProgress.SetUploadSpeed(dataUploadSpeed);
        }

        Debug.Log(
            "Wrong password! Upload speed: " +
            dataUploadSpeed
        );

        OnDataUploadSpeedChanged?.Invoke(dataUploadSpeed);

        UpdateAttemptsText();
        passwordInput.text = "";

        if (attemptsRemaining <= 0)
        {
            passwordInput.interactable = false;

            OnOutOfAttempts?.Invoke();

            Debug.Log("No attempts remaining!");
        }
    }

    private void UpdateAttemptsText()
    {
        attemptsText.text =
            "Attempts remaining: " +
            attemptsRemaining;
    }
}