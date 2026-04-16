using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    public Button playButton;
    public Button quitButton;

    public Button levelBackBtn; 
    public Button soundSettingsButton;
    public Button soundSettingsBackBtn;

    [Header("Level Selection Panel")]
    public GameObject levelSelectPanel;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button endlessButton;

    [Header("Sound Manager")]
    public GameObject SoundSettingsPanel;
     // Optional: Panel for sound settings (if you want to add it in the future)
    private void Awake()
    {
        // Main menu
        playButton.onClick.AddListener(ShowLevelSelect);
        quitButton.onClick.AddListener(QuitGame);
        levelBackBtn.onClick.AddListener(HideLevelSelect); 

        // Sound settings
        soundSettingsButton.onClick.AddListener(ShowSoundSettings);
        soundSettingsBackBtn.onClick.AddListener(HideSoundSettings);

        // Level select buttons
        level1Button.onClick.AddListener(() => PlayLevel(1));
        level2Button.onClick.AddListener(() => PlayLevel(2));
        level3Button.onClick.AddListener(() => PlayLevel(3));
        endlessButton.onClick.AddListener(PlayEndless);

        levelSelectPanel.SetActive(false);
    }
    //LEVEL PANEL SELECT
    private void ShowLevelSelect()
    {
        SoundManager.Instance.PlayButton();
        levelSelectPanel.SetActive(true);
        playButton.gameObject.SetActive(false);
    }

     private void HideLevelSelect()
    {
        SoundManager.Instance.PlayButton();
        levelSelectPanel.SetActive(false);
        playButton.gameObject.SetActive(true);
    }

    // =======================
    // LEVEL SELECTION
    // =======================
    private void PlayLevel(int level)
    {
        SoundManager.Instance.PlayButton();
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
        SoundManager.Instance.PlayButton();
        GameManager.Instance.gameMode = GameManager.GameMode.Endless;
        SceneManager.LoadScene("Playing");
    }


    // =======================
    //SOUND SETTINGS 
    // =======================

    private void ShowSoundSettings()
    {
        SoundManager.Instance.PlayButton();
        SoundSettingsPanel.SetActive(true);
    }

    private void HideSoundSettings()
    {
        SoundManager.Instance.PlayButton();
        SoundSettingsPanel.SetActive(false);
    }

    private void QuitGame()
    {
         SoundManager.Instance.PlayButton();
        Application.Quit();
    }
}