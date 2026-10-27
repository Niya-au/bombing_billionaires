using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PasswordInputUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text attemptsText;

    [SerializeField] private string correctPassword = "MARP5";
    [SerializeField] private int maxAttempts = 3;

    [SerializeField] private float dataUploadSpeed = 1f;
    [SerializeField] private float wrongAttemptSpeedIncrease = 0.5f;

    public UnityEvent<float> OnDataUploadSpeedChanged;
    public UnityEvent OnPasswordCorrect;
    public UnityEvent OnOutOfAttempts;

    private int attemptsRemaining;

    private void Start()
    {
        attemptsRemaining = maxAttempts;
        UpdateAttemptsText();
    }

    public void SubmitPassword()
    {
        // Stop hvis spilleren ikke har flere forsøg
        if (attemptsRemaining <= 0)
        {
            return;
        }

        string attempt = passwordInput.text.Trim();

        // Korrekt password
        if (attempt.Equals(
            correctPassword,
            System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Correct password!");

            OnPasswordCorrect?.Invoke();
            passwordInput.interactable = false;

            return;
        }

        // Forkert password
        attemptsRemaining--;

        // Sørger for at attempts aldrig går under 0
        attemptsRemaining = Mathf.Max(attemptsRemaining, 0);

        // Øg upload-hastigheden
        dataUploadSpeed += wrongAttemptSpeedIncrease;

        Debug.Log(
            "Wrong password! Upload speed: " + dataUploadSpeed
        );

        OnDataUploadSpeedChanged?.Invoke(dataUploadSpeed);

        UpdateAttemptsText();
        passwordInput.text = "";

        // Ingen forsøg tilbage
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
            "Attempts remaining: " + attemptsRemaining;
    }
}