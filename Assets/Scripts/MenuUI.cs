using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class MenuUI : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    public Button playButton;
    public Button quitButton;

    public Button levelBackBtn; 
    public Button soundSettingsButton;
    public Button soundSettingsBackBtn;
    public Button leaderBoardButton;

    [Header("Level Selection Panel")]
    public GameObject levelSelectPanel;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button endlessButton;

    [Header("Level Lock Visuals")]
    public GameObject level2LockIcon;
    public GameObject level3LockIcon;

    [Header("Locked Color Settings")]
    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.white;
    

    [Header("Sound Manager")]
    public GameObject SoundSettingsPanel;
    

     [Header("User")]
    public GameObject userPanel;
    public Button userButton;
    public Button userBackButton;


    [Header("High Scores")]
    public GameObject leaderBoardPanel;
    public Button leaderBoardBackButton;

    public Button changeCategoryButton;
    public GameObject globalCategoryPanel;
    public GameObject userCategoryPanel;

    [Header("Skin Selector")]
    public Button buttonOpenSkinSelector;
    public GameObject skinSelectorPanel;
    public Button skinSelectorBackButton;


    private void Awake()
    {
        // Main menu
        playButton.onClick.AddListener(ShowLevelSelect);
        quitButton.onClick.AddListener(QuitGame);
        levelBackBtn.onClick.AddListener(HideLevelSelect); 

        // Sound settings
        soundSettingsButton.onClick.AddListener(ShowSoundSettings);
        soundSettingsBackBtn.onClick.AddListener(HideSoundSettings);

        // User panel
        userButton.onClick.AddListener(ShowUserPanel);
        userBackButton.onClick.AddListener(HideUserPanel);

        // Leaderboard
        leaderBoardButton.onClick.AddListener(ShowLeaderBoard);
        leaderBoardBackButton.onClick.AddListener(HideLeaderBoard);
        changeCategoryButton.onClick.AddListener(() =>
            {
                if (globalCategoryPanel.activeSelf)
                    ChangeToUser();
                else
                    ChangeToGlobal();
            });

        // Level select buttons
        level1Button.onClick.AddListener(() => PlayLevel(1));
        level2Button.onClick.AddListener(() => PlayLevel(2));
        level3Button.onClick.AddListener(() => PlayLevel(3));
        endlessButton.onClick.AddListener(PlayEndless);

        //skin selector
        buttonOpenSkinSelector.onClick.AddListener(ShowSkinSelector);
        skinSelectorBackButton.onClick.AddListener(HideSkinSelector);

        levelSelectPanel.SetActive(false);
        UpdateLevelButtons();
    }

    void OnEnable()
    {
        UserManager.OnUserChanged += UpdateLevelButtons;
        UpdateLevelButtons(); // IMPORTANT: refresh immediately
    }

    void OnDisable()
    {
        UserManager.OnUserChanged -= UpdateLevelButtons;
    }

    //LEVEL PANEL SELECT
    private void ShowLevelSelect()
    {
        SoundManager.Instance.PlayButton();
        UpdateLevelButtons();
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
        if (LevelProgressManager.Instance != null &&
            !LevelProgressManager.Instance.IsLevelUnlocked(level))
        {
            Debug.Log("Level Locked!");
            return;
        }

        SoundManager.Instance.PlayButton();
        SoundManager.Instance.StopMenuBGM();
        SoundManager.Instance.PlayBGM();

        GameManager.Instance.gameMode = GameManager.GameMode.Level;
        GameManager.Instance.currentLevel = level;

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
    void UpdateLevelButtons()
    {
        if (LevelProgressManager.Instance == null) return;

        // Level 1 always unlocked
        SetLevelVisual(level1Button, true, null);

        // Level 2
        bool level2Unlocked = LevelProgressManager.Instance.IsLevelUnlocked(2);
        SetLevelVisual(level2Button, level2Unlocked, level2LockIcon);

        // Level 3
        bool level3Unlocked = LevelProgressManager.Instance.IsLevelUnlocked(3);
        SetLevelVisual(level3Button, level3Unlocked, level3LockIcon);
    }

    void SetLevelVisual(Button button, bool unlocked, GameObject lockIcon)
    {
        if (button == null) return;

        // Enable / disable interaction
        button.interactable = unlocked;

        // Change button color (grayed if locked)
        Image img = button.GetComponent<Image>();
        if (img != null)
        {
            img.color = unlocked ? unlockedColor : lockedColor;
        }

        // Toggle padlock icon
        if (lockIcon != null)
        {
            lockIcon.SetActive(!unlocked);
        }
    }

    private void PlayEndless()
    {
        SoundManager.Instance.PlayButton();
        SoundManager.Instance.StopMenuBGM();
        SoundManager.Instance.PlayBGM();
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

    // =======================
    // USER PANEL
    // =======================
    public void ShowUserPanel()
    {
        SoundManager.Instance.PlayButton();
        userPanel.SetActive(true);
    }
    public void HideUserPanel()
    {
        SoundManager.Instance.PlayButton();
        userPanel.SetActive(false);
    }

    // =======================
    // LEADERBOARD PANEL
    // =======================
    public void ShowLeaderBoard()
    {
        SoundManager.Instance.PlayButton();
        leaderBoardPanel.SetActive(true);
    }

    public void HideLeaderBoard()
    {
        SoundManager.Instance.PlayButton();
        leaderBoardPanel.SetActive(false);
    }

    public void ChangeToGlobal()
    {
        globalCategoryPanel.SetActive(true);
        userCategoryPanel.SetActive(false);
        changeCategoryButton.GetComponentInChildren<TextMeshProUGUI>().text = "User";
    }

    public void ChangeToUser()
    {
        globalCategoryPanel.SetActive(false);
        userCategoryPanel.SetActive(true);
        changeCategoryButton.GetComponentInChildren<TextMeshProUGUI>().text = "Global";
    }


    // =======================
    // SKIN SELECTOR PANEL
    // =======================
    public void ShowSkinSelector()
    {
        SoundManager.Instance.PlayButton();
        skinSelectorPanel.SetActive(true);
    }

    public void HideSkinSelector()
    {
        SoundManager.Instance.PlayButton();
        skinSelectorPanel.SetActive(false);
    }

    private void QuitGame()
    {
         SoundManager.Instance.PlayButton();
        Application.Quit();
    }
}