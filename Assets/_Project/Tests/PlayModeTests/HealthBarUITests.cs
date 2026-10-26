using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class HealthBarUITests
{
    [UnityTest]
    public IEnumerator HealthBar_DecreasesSmoothly_WhenPlayerTakesDamage()
    {
        // Player + Health
        GameObject player = new GameObject("Player");
        Health health = player.AddComponent<Health>();

        // Fill image
        GameObject fillObject = new GameObject("HealthBarFill");
        Image fillImage = fillObject.AddComponent<Image>();
        fillImage.fillAmount = 1f;

        // HealthBarUI
        GameObject uiObject = new GameObject("HealthBarUI");
        HealthBarUI healthBarUI = uiObject.AddComponent<HealthBarUI>();

        SetPrivateField(healthBarUI, "playerHealth", health);
        SetPrivateField(healthBarUI, "fillImage", fillImage);
        SetPrivateField(healthBarUI, "smoothSpeed", 5f);

        // Lad Start() køre
        yield return null;

        Assert.AreEqual(1f, fillImage.fillAmount, 0.01f);

        // 25 damage = 75 HP
        health.TakeDamage(25);

        // Den må ikke hoppe direkte til 75 %
        Assert.AreEqual(1f, fillImage.fillAmount, 0.01f);

        // Vent lidt
        yield return new WaitForSeconds(0.1f);

        Assert.Less(fillImage.fillAmount, 1f);
        Assert.Greater(fillImage.fillAmount, 0.75f);

        // Vent til animationen næsten er færdig
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(0.75f, fillImage.fillAmount, 0.01f);

        Object.Destroy(player);
        Object.Destroy(fillObject);
        Object.Destroy(uiObject);
    }

    private void SetPrivateField(object target, string fieldName, object value)
    {
        FieldInfo field = target.GetType().GetField(
            fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        field.SetValue(target, value);
    }
}