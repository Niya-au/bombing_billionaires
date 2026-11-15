using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackHitbox;
    public GameObject attackVisual;

    public float attackDuration = 0.12f;
    public float attackLunge = 0.20f;
    public Transform attackVisualTransform;
    public Transform playerVisual;

    private bool isAttacking = false;

    void Start()
    {
        attackHitbox.SetActive(false);
        attackVisual.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            StartCoroutine(DoAttack());
        }
    }

   IEnumerator DoAttack()
    {
        isAttacking = true;

        // Find hvilken vej BK kigger
        float direction = playerVisual.localScale.x < 0 ? -1f : 1f;

        // Flip swoosh
        Vector3 scale = attackVisualTransform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        attackVisualTransform.localScale = scale;

          // Flyt swoosh til den rigtige side
        Vector3 visualPos = attackVisualTransform.localPosition;
        visualPos.x = Mathf.Abs(visualPos.x) * direction;
        attackVisualTransform.localPosition = visualPos;

        // Lunge i den rigtige retning
        transform.position += new Vector3(attackLunge * direction, 0f, 0f);

        attackHitbox.SetActive(true);
        attackVisual.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        attackHitbox.SetActive(false);
        attackVisual.SetActive(false);

        isAttacking = false;
    }
}