using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UserUIManager : MonoBehaviour
{
    [Header("User List")]
    public Transform userListContainer;
    public GameObject userButtonPrefab;
    public TextMeshProUGUI currentUserText;
    public TextMeshProUGUI currentUserBtnText;

    [Header("Create User Panel")]
    public GameObject createPanel;
    public TMP_InputField createInput;
    public Button createConfirmButton;
    public Button createCancelButton;

[Header("Rename Panel")]
    public GameObject renamePanel;
    public TMP_InputField renameInput;
    public Button renameConfirmButton;
    public Button renameCancelButton;

    [Header("Buttons")]
    public Button addUserButton;
    public Button renameUserButton;
    public Button deleteUserButton;

    string selectedUser;

   void Start()
{
    RefreshUserList();
    UpdateCurrentUserUI();

    // MAIN buttons
    addUserButton.onClick.AddListener(OpenCreatePanel);
    renameUserButton.onClick.AddListener(OpenRenamePanel);
    deleteUserButton.onClick.AddListener(DeleteUser);

    // CREATE PANEL BUTTONS
    createConfirmButton.onClick.AddListener(CreateUser);
    createCancelButton.onClick.AddListener(CloseCreatePanel);

    // RENAME PANEL BUTTONS
    renameConfirmButton.onClick.AddListener(ConfirmRename);
    renameCancelButton.onClick.AddListener(CloseRenamePanel);
}

    // ======================
    // USER LIST
    // ======================

    public float entryHeight = 30f;
    public float topPadding = 20f;
   public void RefreshUserList()
    {
        foreach (Transform child in userListContainer)
            Destroy(child.gameObject);

        List<string> users = UserManager.Instance.GetUsers();

        for (int i = 0; i < users.Count; i++)
        {
            GameObject btn = Instantiate(userButtonPrefab, userListContainer);

            RectTransform rt = btn.GetComponent<RectTransform>();

            rt.anchoredPosition = new Vector2(
                0,
                -topPadding - (i * entryHeight)
            );

            UserButton ub = btn.GetComponent<UserButton>();
            ub.username = users[i];
            ub.manager = this;
            ub.SetText(users[i]);

            if (users[i] == UserManager.Instance.GetCurrentUser())
            {
                ub.Highlight(true);
            }
        }

        SetContentHeight(users.Count);
    }

void SetContentHeight(int count)
{
    RectTransform rt = userListContainer.GetComponent<RectTransform>();

    if (rt == null) return;

    rt.sizeDelta = new Vector2(
        rt.sizeDelta.x,
        count * entryHeight
    );
}

    public void SelectUser(string user)
    {
        selectedUser = user;
        UserManager.Instance.SetCurrentUser(user);

        RefreshUserList();
        UpdateCurrentUserUI();
    }

    void UpdateCurrentUserUI()
    {
        currentUserText.text = "Current: " + UserManager.Instance.GetCurrentUser();
        currentUserBtnText.text = UserManager.Instance.GetCurrentUser();
    }

    // ======================
    // CREATE USER
    // ======================
    public void OpenCreatePanel()
    {
        createPanel.SetActive(true);
    }

    public void CreateUser()
    {
        string username = createInput.text;

        if (string.IsNullOrEmpty(username)) return;

        UserManager.Instance.CreateUser(username);

        createInput.text = "";
        createPanel.SetActive(false);

        RefreshUserList();
        UpdateCurrentUserUI();
    }

    // ======================
    // DELETE USER
    // ======================
    public void DeleteUser()
    {
        if (string.IsNullOrEmpty(selectedUser)) return;

        UserManager.Instance.DeleteUser(selectedUser);

        selectedUser = null;

        RefreshUserList();
        UpdateCurrentUserUI();
    }

    // ======================
    // RENAME USER
    // ======================
    public void OpenRenamePanel()
    {
        if (string.IsNullOrEmpty(selectedUser)) return;

        renamePanel.SetActive(true);
        renameInput.text = selectedUser;
    }

    public void ConfirmRename()
    {
        string newName = renameInput.text;

        if (string.IsNullOrEmpty(newName)) return;

        UserManager.Instance.RenameUser(selectedUser, newName);

        selectedUser = newName;

        renamePanel.SetActive(false);

        RefreshUserList();
        UpdateCurrentUserUI();
    }

    public void CloseCreatePanel()
    {
        createPanel.SetActive(false);
    }

    public void CloseRenamePanel()
    {
        renamePanel.SetActive(false);
    }
}