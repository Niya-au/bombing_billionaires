using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackHitbox;
    public GameObject attackVisual;

    public float attackDuration = 0.12f;
    public float attackLunge = 0.20f;

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

        // lille fremadgående bevægelse
        transform.position += new Vector3(attackLunge, 0f, 0f);

        attackHitbox.SetActive(true);
        attackVisual.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        attackHitbox.SetActive(false);
        attackVisual.SetActive(false);

        isAttacking = false;
    }
}