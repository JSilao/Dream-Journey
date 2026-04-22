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

        bool endless = IsEndlessMode(out int level);

        string userKey = GetUserKey(level, endless);
        string globalKey = GetGlobalKey(level, endless);

        int userHighScore = PlayerPrefs.GetInt(userKey, 0);
        int globalHighScore = PlayerPrefs.GetInt(globalKey, 0);

        // Update USER score
        if (distance > userHighScore)
        {
            userHighScore = distance;
            PlayerPrefs.SetInt(userKey, userHighScore);
        }

        // Update GLOBAL score
        if (distance > globalHighScore)
        {
            globalHighScore = distance;
            PlayerPrefs.SetInt(globalKey, globalHighScore);
        }

        PlayerPrefs.Save();

        // =========================
        // UI OUTPUT (STRICT MODE SEPARATION)
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
                "\nLevel: " + level +
                "\nDistance: " + distance + " m" +
                "\nYour Best (Level " + level + "): " + userHighScore + " m" +
                "\nGlobal Best (Level " + level + "): " + globalHighScore + " m";
        }

        player.velocity = Vector2.zero;

        string user = GetUser();
        if(endless)
    {
        LeaderboardManager.Instance.AddScore(user, distance);}

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

    public void Retry()
    {
        SoundManager.Instance.PlayButton();
        SoundManager.Instance.repeatBGM();
        SceneManager.LoadScene("Playing");
    }
}