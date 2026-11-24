
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float offsetX = 0f;

    private float fixedY;
    private float fixedZ;

    void Start()
    {
        // Remember the camera's starting height and depth.
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Follow the player horizontally only.
        Vector3 desiredPosition = new Vector3(
            target.position.x + offsetX,
            fixedY,
            fixedZ
        );

        // Smoothly move the camera toward the player.
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            Mathf.Clamp01(smoothSpeed * Time.deltaTime)
        );
    }
}
