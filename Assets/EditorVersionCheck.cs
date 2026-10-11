using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class EditorVersionCheck
{
    static EditorVersionCheck()
    {
        string requiredVersion = "6000.3.22f1";

        if (Application.unityVersion != requiredVersion)
        {
            EditorUtility.DisplayDialog(
                "Incorrect Unity Version",
                $"This project requires Unity {requiredVersion}. You are currently using {Application.unityVersion}.",
                "OK"
            );
        }
    }
}