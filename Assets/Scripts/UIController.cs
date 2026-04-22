using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Player & UI References")]
    public Player player;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI healthText;
    public GameObject results;
    public TextMeshProUGUI finalDistanceText;

    [Header("Buttons")]
    public Button quitButton;
    public Button retryButton;
    public Button nextLevelButton; 
    [Header("Next Level Lock")]
public GameObject nextLevelLockIcon;

    void Awake()
    {
        if (player == null)
            player = GameObject.Find("Player").GetComponent<Player>();

        if (results != null)
            results.SetActive(false);

        if (quitButton != null)
            quitButton.onClick.AddListener(Quit);

        if (retryButton != null)
            retryButton.onClick.AddListener(Retry);

        if (nextLevelButton != null)
    {
        nextLevelButton.onClick.AddListener(NextLevel);
        nextLevelButton.gameObject.SetActive(false); // hidden by default
    }
    }

    void Update()
    {
        if (player == null) return;

        int distance = Mathf.FloorToInt(player.distance);

        if (distanceText != null)
            distanceText.text = distance + " m";

        if (healthText != null)
            healthText.text = "HP: " + player.health;

        if (player.isDead)
        {
            ShowResults("Game Over!", distance);
        }

        if (player.levelCompleted)
        {
            ShowResults("Level Complete!", distance);
        }
    }

    // =========================
    // USER + MODE HELPERS
    // =========================

    string GetUser()
    {
        return PlayerPrefs.GetString("CurrentUser", "Guest");
    }

    bool IsEndlessMode(out int level)
    {
        level = 1;

        if (GameManager.Instance == null)
            return false;

        level = GameManager.Instance.currentLevel;
        return GameManager.Instance.gameMode == GameManager.GameMode.Endless;
    }

    // =========================
    // SCORE KEYS
    // =========================

    string GetUserKey(int level, bool endless)
    {
        string user = GetUser();

        if (endless)
            return "HighScore_Endless_" + user;

        return "HighScore_Level_" + level + "_" + user;
    }

    string GetGlobalKey(int level, bool endless)
    {
        if (endless)
            return "HighScore_Endless_GLOBAL";

        return "HighScore_Level_" + level + "_GLOBAL";
    }

    // =========================
    // MAIN RESULTS LOGIC
    // =========================

    void ShowResults(string message, int distance)
    {
        if (results == null || results.activeSelf) return;

        results.SetActive(true);

        // =========================
        // LEVEL STATE
        // =========================
        bool endless = IsEndlessMode(out int currentLevel);

        int completedLevel = GameManager.Instance.currentLevel;
        int nextLevel = completedLevel + 1;

        // =========================
        // NEXT LEVEL BUTTON
        // =========================
        if (nextLevelButton != null)
        {
            bool hasNextLevel = !endless && completedLevel < 3;

            nextLevelButton.gameObject.SetActive(hasNextLevel);

            if (hasNextLevel)
            {
                bool canProceed = true;

                if (LevelProgressManager.Instance != null)
                {
                    int unlockedLevel = LevelProgressManager.Instance.GetUnlockedLevel();
                    canProceed = nextLevel <= unlockedLevel;
                }

                nextLevelButton.interactable = canProceed;

                Image img = nextLevelButton.GetComponent<Image>();
                if (img == null)
                    img = nextLevelButton.GetComponentInChildren<Image>();

                if (img != null)
                    img.color = canProceed ? Color.white : Color.gray;

                TextMeshProUGUI txt = nextLevelButton.GetComponentInChildren<TextMeshProUGUI>();
                if (txt != null)
                    txt.color = canProceed ? Color.white : Color.gray;

                if (nextLevelLockIcon != null)
                    nextLevelLockIcon.SetActive(!canProceed);
            }
        }

        // =========================
        // SCORES
        // =========================
        string userKey = GetUserKey(completedLevel, endless);
        string globalKey = GetGlobalKey(completedLevel, endless);

        int userHighScore = PlayerPrefs.GetInt(userKey, 0);
        int globalHighScore = PlayerPrefs.GetInt(globalKey, 0);

        if (distance > userHighScore)
        {
            userHighScore = distance;
            PlayerPrefs.SetInt(userKey, userHighScore);
        }

        if (distance > globalHighScore)
        {
            globalHighScore = distance;
            PlayerPrefs.SetInt(globalKey, globalHighScore);
        }

        PlayerPrefs.Save();

        // =========================
        // UI OUTPUT
        // =========================
        if (endless)
        {
            finalDistanceText.text =
                message +
                "\nDistance: " + distance + " m" +
                "\nYour Best (Endless): " + userHighScore + " m" +
                "\nGlobal Best (Endless): " + globalHighScore + " m";
        }
        else
        {
            finalDistanceText.text =
                message +
                "\nLevel: " + completedLevel +
                "\nDistance: " + distance + " m" +
                "\nYour Best (Level " + completedLevel + "): " + userHighScore + " m" +
                "\nGlobal Best (Level " + completedLevel + "): " + globalHighScore + " m";
        }

        player.velocity = Vector2.zero;

        if (endless && LeaderboardManager.Instance != null)
        {
            LeaderboardManager.Instance.AddScore(GetUser(), distance);
        }
    }

    // =========================
    // BUTTONS
    // =========================

    public void Quit()
    {
        SoundManager.Instance.PlayButton();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayMenuBGM();
        SceneManager.LoadScene("Menu");
    }
     public void NextLevel()
    {
        SoundManager.Instance.PlayButton();

        if (GameManager.Instance == null) return;

        int nextLevel = GameManager.Instance.currentLevel + 1;

        if (nextLevel > 3) return;

        if (LevelProgressManager.Instance != null)
        {
            int unlockedLevel = LevelProgressManager.Instance.GetUnlockedLevel();

            if (nextLevel > unlockedLevel)
            {
                Debug.Log("Next level is locked!");
                return;
            }
        }

        GameManager.Instance.currentLevel = nextLevel;

        switch (nextLevel)
        {
            case 1:
                GameManager.Instance.dayDistance = 500;
                GameManager.Instance.afternoonDistance = 0;
                GameManager.Instance.nightDistance = 0;
                break;
            case 2:
                GameManager.Instance.dayDistance = 500;
                GameManager.Instance.afternoonDistance = 500;
                GameManager.Instance.nightDistance = 0;
                break;
            case 3:
                GameManager.Instance.dayDistance = 500;
                GameManager.Instance.afternoonDistance = 500;
                GameManager.Instance.nightDistance = 500;
                break;
        }

        SceneManager.LoadScene("Playing");
    }


    public void Retry()
    {
        SoundManager.Instance.PlayButton();
        SoundManager.Instance.repeatBGM();
        SceneManager.LoadScene("Playing");
    }
}