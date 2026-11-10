using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void ContinueButton()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}