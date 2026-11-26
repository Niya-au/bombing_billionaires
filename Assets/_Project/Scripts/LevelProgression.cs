
using UnityEngine;

public static class LevelProgression
{
    private const string ProgressKey = "HighestUnlockedLevel";
    private const int MaxLevel = 5;

    private static int highestUnlockedLevel = 1;

    public static int HighestUnlockedLevel
    {
        get { return highestUnlockedLevel; }
    }

    [RuntimeInitializeOnLoadMethod(
        RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Initialize()
    {
#if UNITY_EDITOR
        // Reset progression when entering Play mode.
        highestUnlockedLevel = 1;
#else
        // Load saved progression in the packaged game.
        highestUnlockedLevel = Mathf.Clamp(
            PlayerPrefs.GetInt(ProgressKey, 1),
            1,
            MaxLevel
        );
#endif
    }

    public static bool IsLevelUnlocked(int level)
    {
        return level >= 1 &&
               level <= highestUnlockedLevel;
    }

    public static void CompleteLevel(int completedLevel)
    {
        if (!IsLevelUnlocked(completedLevel))
            return;

        int nextLevel = Mathf.Min(
            completedLevel + 1,
            MaxLevel
        );

        if (nextLevel > highestUnlockedLevel)
        {
            highestUnlockedLevel = nextLevel;

            SaveProgress();

            Debug.Log("Level " + nextLevel + " unlocked!");
        }
    }

    private static void SaveProgress()
    {
#if !UNITY_EDITOR
        PlayerPrefs.SetInt(
            ProgressKey,
            highestUnlockedLevel
        );

        PlayerPrefs.Save();
#endif
    }

    public static void ResetProgress()
    {
        highestUnlockedLevel = 1;

        PlayerPrefs.DeleteKey(ProgressKey);
        PlayerPrefs.Save();

        Debug.Log("Level progression reset.");
    }
}
