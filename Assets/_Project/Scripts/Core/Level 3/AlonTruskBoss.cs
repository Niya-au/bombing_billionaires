
using UnityEngine;
using UnityEngine.Events;

public class AlonTruskBoss : BossBase
{
    [Header("Movement")]
    [SerializeField] private float moveRadiusX = 3f;
    [SerializeField] private float moveRadiusY = 1.5f;
    [SerializeField] private float movementSpeed = 1f;

    [Header("Shooting")]
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Boss Events")]
    public UnityEvent OnAlonDefeated;

    private Vector3 startPosition;
    private float movementTimer;
    private float shootTimer;
    private bool defeated = false;

    protected override void Start()
    {
        base.Start();

        startPosition = transform.position;
    }

    private void Update()
    {
        if (defeated || currentHP <= 0)
            return;

        MoveAround();
        ShootRockets();
    }

    private void MoveAround()
    {
        movementTimer += Time.deltaTime;

        float phaseSpeed = movementSpeed;

        if (currentPhase == 2)
            phaseSpeed *= 1.5f;
        else if (currentPhase == 3)
            phaseSpeed *= 2f;

        float x = Mathf.Sin(
            movementTimer * phaseSpeed
        ) * moveRadiusX;

        float y = Mathf.Sin(
            movementTimer * phaseSpeed * 0.7f
        ) * moveRadiusY;

        transform.position = startPosition + new Vector3(x, y, 0f);
    }

    private void ShootRockets()
    {
        shootTimer += Time.deltaTime;

        float fireInterval = 2.5f;

        if (currentPhase == 2)
            fireInterval = 1.5f;
        else if (currentPhase == 3)
            fireInterval = 0.8f;

        if (shootTimer >= fireInterval)
        {
            FireRocket();
            shootTimer = 0f;
        }
    }

    private void FireRocket()
    {
        if (rocketPrefab == null || firePoint == null)
            return;

        Instantiate(
            rocketPrefab,
            firePoint.position,
            firePoint.rotation
        );
    }

    protected override void OnPhaseChange(int newPhase)
    {
        base.OnPhaseChange(newPhase);

        Debug.Log("Alon entered phase " + newPhase);
    }

    protected override void OnDefeated()
    {
        if (defeated)
            return;

        defeated = true;

        base.OnDefeated();
        Die();
    }

    protected override void Die()
    {
        OnAlonDefeated?.Invoke();
        gameObject.SetActive(false);
    }
}
