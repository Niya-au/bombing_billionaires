
using UnityEngine;

public static class LevelProgression
{
    private const string ProgressKey = "HighestUnlockedLevel";
    private const int MaxLevel = 5;

    public static int HighestUnlockedLevel
    {
        get
        {
            return Mathf.Clamp(
                PlayerPrefs.GetInt(ProgressKey, 1),
                1,
                MaxLevel
            );
        }
    }

    public static bool IsLevelUnlocked(int level)
    {
        return level >= 1 &&
               level <= HighestUnlockedLevel;
    }

    public static void CompleteLevel(int completedLevel)
    {
        if (!IsLevelUnlocked(completedLevel))
            return;

        int nextLevel = Mathf.Min(
            completedLevel + 1,
            MaxLevel
        );

        if (nextLevel > HighestUnlockedLevel)
        {
            PlayerPrefs.SetInt(ProgressKey, nextLevel);
            PlayerPrefs.Save();

            Debug.Log("Level " + nextLevel + " unlocked!");
        }
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(ProgressKey);
        PlayerPrefs.Save();

        Debug.Log("Level progression reset.");
    }
}
