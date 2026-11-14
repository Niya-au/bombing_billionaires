using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public BoxCollider2D playerCollider;
    public Transform playerVisual;

    private Vector2 normalSize;
    private Vector2 normalOffset;

    private Vector3 normalVisualPosition;
    private Vector3 normalVisualScale;

    public Vector2 crouchSize = new Vector2(1f, 0.8f);
    public Vector2 crouchOffset = new Vector2(0f, -0.25f);

    public float visualDownAmount = 0.15f;
    public float crouchScaleY = 0.92f;

    private bool isCrouching = false;

    void Start()
    {
        normalSize = playerCollider.size;
        normalOffset = playerCollider.offset;

        normalVisualPosition = playerVisual.localPosition;
        normalVisualScale = playerVisual.localScale;
    }

    void Update()
    {
        bool crouchPressed =
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.DownArrow);

        if (crouchPressed && !isCrouching)
        {
            Crouch();
        }
        else if (!crouchPressed && isCrouching)
        {
            StandUp();
        }
    }

    void Crouch()
    {
        isCrouching = true;

        // Gør collideren lavere
        playerCollider.size = crouchSize;
        playerCollider.offset = crouchOffset;

        // Flyt visual lidt ned
        playerVisual.localPosition =
            normalVisualPosition + new Vector3(0f, -visualDownAmount, 0f);

        // Kun en lille squash
        playerVisual.localScale =
            new Vector3(
                normalVisualScale.x,
                normalVisualScale.y * crouchScaleY,
                normalVisualScale.z
            );
    }

    void StandUp()
    {
        isCrouching = false;

        // Gendan collider
        playerCollider.size = normalSize;
        playerCollider.offset = normalOffset;

        // Gendan visual
        playerVisual.localPosition = normalVisualPosition;
        playerVisual.localScale = normalVisualScale;
    }
}