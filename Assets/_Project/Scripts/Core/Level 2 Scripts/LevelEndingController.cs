
using System.Collections;
using UnityEngine;

public class LevelEndingController : MonoBehaviour
{
    [Header("Player")]
    public MonoBehaviour[] playerControlScripts;

    [Header("Camera")]
    public Transform mainCamera;
    public MonoBehaviour cameraFollowScript;
    public Transform jaffEscapePoint;

    [Header("Jaff Escape")]
    public GameObject escapeJaffDrone;
    public GameObject normalJaff;
    public float droneFlySpeed = 3f;
    public float escapeDuration = 4f;

    [Header("Ending UI")]
    public GameObject escapePanel;
    public GameObject successPanel;

    private bool endingStarted = false;

    void Start()
    {
        if (escapePanel != null)
            escapePanel.SetActive(false);

        if (successPanel != null)
            successPanel.SetActive(false);

        if (escapeJaffDrone != null)
            escapeJaffDrone.SetActive(false);
    }

    public void TriggerJaffEscape()
    {
        if (endingStarted)
            return;

        endingStarted = true;
        StopPlayerControls();

        StartCoroutine(JaffEscapeSequence());
    }


    IEnumerator JaffEscapeSequence()
    {
        // Stop normal camera following.
        if (cameraFollowScript != null)
            cameraFollowScript.enabled = false;

        // Position the drone where Jaff currently is.
        if (escapeJaffDrone != null)
        {
            if (normalJaff != null)
            {
                escapeJaffDrone.transform.position =
                    normalJaff.transform.position;
            }

            escapeJaffDrone.SetActive(true);

            Debug.Log("Escape drone activated at: " +
                      escapeJaffDrone.transform.position);
        }
        else
        {
            Debug.LogError("Escape Jaff Drone is not assigned!");
            yield break;
        }

        // Hide normal Jaff after positioning the drone.
        if (normalJaff != null)
            normalJaff.SetActive(false);

        // Position the camera on the drone.
        if (mainCamera != null)
        {
            mainCamera.position = new Vector3(
                escapeJaffDrone.transform.position.x,
                escapeJaffDrone.transform.position.y,
                mainCamera.position.z
            );
        }

        // Fly upward while the camera follows.
        float elapsed = 0f;

        while (elapsed < escapeDuration)
        {
            escapeJaffDrone.transform.position +=
                Vector3.up * droneFlySpeed * Time.deltaTime;

            if (mainCamera != null)
            {
                Vector3 targetPosition = new Vector3(
                    escapeJaffDrone.transform.position.x,
                    escapeJaffDrone.transform.position.y,
                    mainCamera.position.z
                );

                mainCamera.position = Vector3.Lerp(
                    mainCamera.position,
                    targetPosition,
                    4f * Time.deltaTime
                );
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Show the escape ending.
        if (escapePanel != null)
            escapePanel.SetActive(true);
    }


    public void TriggerMissionSuccess()
    {
        if (endingStarted)
            return;

        endingStarted = true;
        StopPlayerControls();

        if (successPanel != null)
            successPanel.SetActive(true);
    }

    void StopPlayerControls()
    {
        foreach (MonoBehaviour script in playerControlScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        // Keep time running for the escape animation.
        Time.timeScale = 1f;
    }
}
