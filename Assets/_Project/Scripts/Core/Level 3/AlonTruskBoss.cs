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

    [SerializeField, Min(0.1f)] private float phase1Interval = 2.5f;
    [SerializeField, Min(0.1f)] private float phase2Interval = 1.5f;
    [SerializeField, Min(0.1f)] private float phase3Interval = 0.8f;

    [Header("Boss Events")]
    public UnityEvent OnAlonDefeated;

    private Vector3 startPosition;
    private float movementTimer;
    private float movementAngle;
    private float shootTimer;

    private bool fightStarted;
    private bool defeated;
    private bool deathHandled;

    protected override void Start()
    {
        base.Start();

        // Combat starts only when the manager calls StartBossFight().
        // Do not reset fightStarted here: the manager may have
        // already called it during this object's activation.
    }

    public void StartBossFight()
    {
        if (defeated || fightStarted)
            return;

        // Patrol around the position where Alon emerged.
        startPosition = transform.position;

        movementTimer = 0f;
        movementAngle = 0f;
        shootTimer = 0f;
        fightStarted = true;
    }

    public void StopBossFight()
    {
        fightStarted = false;
        shootTimer = 0f;
    }

    private void Update()
    {
        if (!fightStarted || defeated || currentHP <= 0)
            return;

        MoveAround();
        ShootRockets();
    }

    private void MoveAround()
    {
        movementTimer += Time.deltaTime;

        float speedMultiplier = 1f;

        if (currentPhase == 2)
            speedMultiplier = 1.5f;
        else if (currentPhase == 3)
            speedMultiplier = 2f;

        // Accumulate the angle so changing phase doesn't
        // suddenly teleport Alon to another patrol position.
        movementAngle +=
            movementSpeed * speedMultiplier * Time.deltaTime;

        float x = Mathf.Sin(movementAngle) * moveRadiusX;
        float y = Mathf.Sin(movementAngle * 0.7f) * moveRadiusY;

        transform.position =
            startPosition + new Vector3(x, y, 0f);
    }

    private void ShootRockets()
    {
        float interval = phase1Interval;

        if (currentPhase == 2)
            interval = phase2Interval;
        else if (currentPhase == 3)
            interval = phase3Interval;

        shootTimer += Time.deltaTime;

        if (shootTimer >= interval)
        {
            shootTimer = 0f;
            FireRocket();
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
        StopBossFight();

        base.OnDefeated();
        Die();
    }

    protected override void Die()
    {
        // Protect against BossBase also calling Die().
        if (deathHandled)
            return;

        deathHandled = true;
        defeated = true;
        StopBossFight();

        OnAlonDefeated?.Invoke();
        gameObject.SetActive(false);
    }
}