
using UnityEngine;

public class SpaceDrift : MonoBehaviour
{
    [Header("Floating Movement")]
    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 1.8f;
    [SerializeField] private float directionChangeInterval = 2f;
    [SerializeField] private float directionSmoothness = 2f;

    [Header("Lifetime")]
    [SerializeField] private float minLifetime = 4f;
    [SerializeField] private float maxLifetime = 9f;

    [Header("Rotation")]
    [SerializeField] private bool rotateWhileFloating = true;
    [SerializeField] private float rotationSpeed = 25f;

    private Vector2 currentDirection;
    private Vector2 targetDirection;
    private float speed;
    private float changeTimer;
    private float rotationDirection;

    private void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);

        currentDirection = Random.insideUnitCircle.normalized;

        if (currentDirection == Vector2.zero)
            currentDirection = Vector2.up;

        targetDirection = currentDirection;

        rotationDirection = Random.value < 0.5f ? -1f : 1f;

        changeTimer = directionChangeInterval;

        Destroy(gameObject, Random.Range(minLifetime, maxLifetime));
    }

    private void Update()
    {
        changeTimer -= Time.deltaTime;

        if (changeTimer <= 0f)
        {
            targetDirection = Random.insideUnitCircle.normalized;
            changeTimer = directionChangeInterval;
        }

        currentDirection = Vector2.Lerp(
            currentDirection,
            targetDirection,
            directionSmoothness * Time.deltaTime
        );

        transform.position +=
            (Vector3)(currentDirection * speed * Time.deltaTime);

        if (rotateWhileFloating)
        {
            transform.Rotate(
                0f,
                0f,
                rotationSpeed * rotationDirection * Time.deltaTime
            );
        }
    }
}

