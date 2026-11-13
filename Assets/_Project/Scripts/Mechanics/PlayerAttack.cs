using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackHitbox;

    void Start()
    {
        attackHitbox.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            attackHitbox.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            attackHitbox.SetActive(false);
        }
    }
}