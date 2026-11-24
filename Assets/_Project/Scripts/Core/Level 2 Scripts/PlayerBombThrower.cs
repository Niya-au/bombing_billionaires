
using UnityEngine;

public class PlayerBombThrower : MonoBehaviour
{
    [Header("Bomb")]
    public GameObject bombPrefab;
    public Transform throwPoint;
    public float throwForce = 12f;
    public float throwCooldown = 0.8f;

    [Header("Swing Momentum")]
    public float momentumMultiplier = 0.5f;

    private Rigidbody2D playerRb;
    private Camera mainCamera;
    private float nextThrowTime;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            Time.time >= nextThrowTime)
        {
            ThrowBomb();
        }
    }

    void ThrowBomb()
    {
        if (bombPrefab == null ||
            throwPoint == null ||
            mainCamera == null)
            return;

        Vector3 mouseWorld =
            mainCamera.ScreenToWorldPoint(
                Input.mousePosition
            );

        Vector2 direction = (
            mouseWorld - throwPoint.position
        ).normalized;

        GameObject bomb = Instantiate(
            bombPrefab,
            throwPoint.position,
            Quaternion.identity
        );

        Rigidbody2D bombRb =
            bomb.GetComponent<Rigidbody2D>();

        if (bombRb != null)
        {
            Vector2 momentum = Vector2.zero;

            if (playerRb != null)
                momentum =
                    playerRb.linearVelocity *
                    momentumMultiplier;

            bombRb.linearVelocity =
                direction * throwForce + momentum;
        }

        nextThrowTime =
            Time.time + throwCooldown;
    }
}
