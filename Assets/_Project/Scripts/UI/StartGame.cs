using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Intro → LevelSelect
    public void ContinueButton()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    // LevelSelect → Level 1
    public void StartLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }
}