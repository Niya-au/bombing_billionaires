
using System.Collections;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerController playerController;

    private bool isDead = false;

    private void Start()
    {
        if (playerHealth != null)
            playerHealth.OnDied.AddListener(HandleDeath);
    }

    private void HandleDeath()
    {
        if (isDead)
            return;

        isDead = true;

        if (playerController != null)
            playerController.enabled = false;

        if (playerAnimator != null)
            playerAnimator.Play("PlayerDeath", 0, 0f);

        Debug.Log("Tyrone is defeated!");

        StartCoroutine(DisappearAfterDeath());
    }

    private IEnumerator DisappearAfterDeath()
    {
        // Wait for the death animation.
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnDied.RemoveListener(HandleDeath);
    }
}
