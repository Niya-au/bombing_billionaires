using UnityEngine;

public class VineController : MonoBehaviour
{
    [Header("Vine Settings")]
    public float vineLength = 5f;

    [Header("Swing Limits")]
    public float maxSwingAngle = 60f;


    private Transform player;
    private bool playerAttached;

    public void AttachPlayer(Transform playerTransform)
    {
        player = playerTransform;
        playerAttached = true;
    }

    public void DetachPlayer()
    {
        playerAttached = false;
        player = null;
        transform.rotation = Quaternion.identity;
    }

    void LateUpdate()
    {
        if (!playerAttached || player == null)
            return;

        Vector2 direction =
            player.position - transform.position;


        float angle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg + 90f;

        // Normalize angle to -180 to 180 degrees
        angle = Mathf.DeltaAngle(0f, angle);

        // Limit the vine's rotation
        angle = Mathf.Clamp(
            angle,
            -maxSwingAngle,
            maxSwingAngle
        );

        transform.rotation = Quaternion.Euler(
            0f, 0f, angle
        );

    }
}
