using UnityEngine;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    string GetUser()
    {
        return PlayerPrefs.GetString("CurrentUser", "Guest");
    }

    string GetKey()
    {
        return "UnlockedLevel_" + GetUser();
    }

    // =========================
    // GET UNLOCKED LEVEL
    // =========================
    public int GetUnlockedLevel()
    {
        return PlayerPrefs.GetInt(GetKey(), 1); // Level 1 default
    }

    // =========================
    // UNLOCK NEXT LEVEL
    // =========================
    public void UnlockNextLevel(int completedLevel)
    {
        int unlocked = GetUnlockedLevel();

        if (completedLevel >= unlocked)
        {
            unlocked = completedLevel + 1;
            PlayerPrefs.SetInt(GetKey(), unlocked);
            PlayerPrefs.Save();

            Debug.Log("Unlocked Level: " + unlocked);
        }
    }

    // =========================
    // CHECK IF LEVEL IS UNLOCKED
    // =========================
    public bool IsLevelUnlocked(int level)
    {
        return level <= GetUnlockedLevel();
    }

    // =========================
    // RESET (optional utility)
    // =========================
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(GetKey());
    }
}