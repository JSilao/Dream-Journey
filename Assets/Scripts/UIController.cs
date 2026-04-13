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

    int highScore;

    void Awake()
    {
        // Assign high score
        highScore = PlayerPrefs.GetInt("HighScore",0);

        // Ensure player is assigned
        if(player == null)
            player = GameObject.Find("Player").GetComponent<Player>();

        // Results panel should start inactive
        if(results != null)
            results.SetActive(false);

        // Add listeners for buttons
        if(quitButton != null)
            quitButton.onClick.AddListener(Quit);
        if(retryButton != null)
            retryButton.onClick.AddListener(Retry);
    }

    void Update()
{
    if(player == null) return;

    int distance = Mathf.FloorToInt(player.distance);

    if(distanceText != null)
        distanceText.text = distance + " m";

    if(healthText != null)
        healthText.text = "HP: " + player.health;

    // Death
    if(player.isDead)
    {
        ShowResults("Game Over!", distance);
    }

    // Level complete
    if(player.levelCompleted)
    {
        ShowResults("Level Complete!", distance);
    }
}
    string GetHighScoreKey()
    {
        if(GameManager.Instance.gameMode == GameManager.GameMode.Endless)
        {
            return "HighScore_Endless";
        }

        return "HighScore_Level_" + GameManager.Instance.currentLevel;
    }

   void ShowResults(string message, int distance)
    {
        if(!results.activeSelf)
        {
            results.SetActive(true);

            string key = GetHighScoreKey();
            int highScore = PlayerPrefs.GetInt(key, 0);

            if(distance > highScore)
            {
                highScore = distance;
                PlayerPrefs.SetInt(key, highScore);
            }

            if(finalDistanceText != null)
            {
                finalDistanceText.text =
                    message +
                    "\nDistance: " + distance + " m" +
                    "\nHigh Score: " + highScore + " m";
            }

            player.velocity = Vector2.zero;
        }
    }

    public void Quit()
    {
        SoundManager.Instance.PlayButton();
        SceneManager.LoadScene("Menu");   
    }

    public void Retry()
    {
        SoundManager.Instance.PlayButton();
        SceneManager.LoadScene("Playing");
    }
}