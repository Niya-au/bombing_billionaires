
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [System.Serializable]
    public class LevelCard
    {
        public Button startButton;
        public GameObject lockedOverlay;
    }

    [Header("Level Cards - New Campaign Order")]
    [SerializeField] private LevelCard[] levelCards;

    // Maps campaign levels to existing Unity scenes.
    private static readonly string[] levelSceneNames =
    {
        "Level 1", // Ronald Dump
        "Level 3", // Alon Trusk
        "Level 4", // Marp Suckerberg
        "Level 5", // Nepstein
        "Level 2"  // Jaff Bezor
    };

    private void Start()
    {
        RefreshLevelCards();
    }

    public void RefreshLevelCards()
    {
        if (levelCards == null)
            return;

        for (int i = 0; i < levelCards.Length; i++)
        {
            int levelNumber = i + 1;

            bool isUnlocked =
                LevelProgression.IsLevelUnlocked(levelNumber);

            if (levelCards[i] == null)
                continue;

            if (levelCards[i].startButton != null)
            {
                levelCards[i].startButton.interactable =
                    isUnlocked;
            }

            if (levelCards[i].lockedOverlay != null)
            {
                levelCards[i].lockedOverlay.SetActive(
                    !isUnlocked
                );
            }
        }
    }

    // Intro -> Backstory
    public void ContinueButton()
    {
        SceneManager.LoadScene("Backstory");
    }

    public void StartLevel1()
    {
        LoadLevel(1);
    }

    public void StartLevel2()
    {
        LoadLevel(2);
    }

    public void StartLevel3()
    {
        LoadLevel(3);
    }

    public void StartLevel4()
    {
        LoadLevel(4);
    }

    public void StartLevel5()
    {
        LoadLevel(5);
    }

    private void LoadLevel(int levelNumber)
    {
        if (levelNumber < 1 ||
            levelNumber > levelSceneNames.Length)
        {
            Debug.LogError("Invalid level number!");
            return;
        }

        if (!LevelProgression.IsLevelUnlocked(levelNumber))
        {
            Debug.Log("Level " + levelNumber + " is locked!");
            return;
        }

        Time.timeScale = 1f;

        string sceneName = levelSceneNames[levelNumber - 1];

        Debug.Log("Loading campaign level " +
                  levelNumber + ": " + sceneName);

        SceneManager.LoadScene(sceneName);
    }

    [ContextMenu("Reset Level Progress")]
    public void ResetLevelProgress()
    {
        LevelProgression.ResetProgress();
        RefreshLevelCards();
    }
}
