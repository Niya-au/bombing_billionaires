using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Intro → LevelSelect
    public void ContinueButton()
    {
        SceneManager.LoadScene("Backstory");
    }

    // LevelSelect → Level 1
    public void StartLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }

    // LevelSelect → Level 2
    public void StartLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }

    // LevelSelect → Level 3
    public void StartLevel3()
    {
        SceneManager.LoadScene("Level 3");
    }

    // LevelSelect → Level 4
    public void StartLevel4()
    {
        SceneManager.LoadScene("Level 4");
    }

    // LevelSelect → Level 5
    public void StartLevel5()
    {
        SceneManager.LoadScene("Level 5");
    }
}