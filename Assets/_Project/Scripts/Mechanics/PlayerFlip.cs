using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    public Transform playerVisual;
    public Rigidbody2D rb;

    void Update()
    {
        if (rb.linearVelocity.x > 0.1f)
        {
            Vector3 scale = playerVisual.localScale;
            scale.x = Mathf.Abs(scale.x);
            playerVisual.localScale = scale;
        }
        else if (rb.linearVelocity.x < -0.1f)
        {
            Vector3 scale = playerVisual.localScale;
            scale.x = -Mathf.Abs(scale.x);
            playerVisual.localScale = scale;
        }
    }
}