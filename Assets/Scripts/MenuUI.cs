using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    public Button playButton;
    public Button quitButton;

    [Header("Level Selection Panel")]
    public GameObject levelSelectPanel;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button endlessButton;

    private void Awake()
    {
        // Main menu
        playButton.onClick.AddListener(ShowLevelSelect);
        quitButton.onClick.AddListener(QuitGame);

        // Level select buttons
        level1Button.onClick.AddListener(() => PlayLevel(1));
        level2Button.onClick.AddListener(() => PlayLevel(2));
        level3Button.onClick.AddListener(() => PlayLevel(3));
        endlessButton.onClick.AddListener(PlayEndless);

        levelSelectPanel.SetActive(false);
    }

    private void ShowLevelSelect()
    {
        levelSelectPanel.SetActive(true);
        playButton.gameObject.SetActive(false);
    }

    private void PlayLevel(int level)
    {
        GameManager.Instance.gameMode = GameManager.GameMode.Level;
        GameManager.Instance.currentLevel = level;

        // Set level distances
        switch(level)
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

    private void PlayEndless()
    {
        GameManager.Instance.gameMode = GameManager.GameMode.Endless;
        SceneManager.LoadScene("Playing");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}