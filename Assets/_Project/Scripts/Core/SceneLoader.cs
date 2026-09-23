using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
