
using UnityEngine;

public class VineVisual : MonoBehaviour
{
    public float swingAngle = 30f;
    public float swingSpeed = 2f;

    void Update()
    {
        float angle = Mathf.Sin(
            Time.time * swingSpeed
        ) * swingAngle;

        transform.rotation = Quaternion.Euler(
            0f, 0f, angle
        );
    }
}
