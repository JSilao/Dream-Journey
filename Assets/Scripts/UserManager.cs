using UnityEngine;
using System.Collections.Generic;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance;

    private List<string> users = new List<string>();
    public static System.Action OnUserChanged;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadUsers();

        // Ensure at least one user exists
        if (users.Count == 0)
        {
            CreateUser("Player1");
        }
    }

    void LoadUsers()
    {
        string data = PlayerPrefs.GetString("Users", "");
        if (!string.IsNullOrEmpty(data))
        {
            users = new List<string>(data.Split(','));
        }
    }

    void SaveUsers()
    {
        PlayerPrefs.SetString("Users", string.Join(",", users));
        PlayerPrefs.Save();
    }

    public List<string> GetUsers()
    {
        return users;
    }

    public void CreateUser(string username)
    {
        if (string.IsNullOrEmpty(username) || users.Contains(username)) return;

        users.Add(username);
        SaveUsers();
        SetCurrentUser(username);
    }

    public void DeleteUser(string username)
    {
        if (!users.Contains(username)) return;

        users.Remove(username);
        SaveUsers();

        if (GetCurrentUser() == username)
        {
            SetCurrentUser(users.Count > 0 ? users[0] : "Guest");
        }
    }

   public void RenameUser(string oldName, string newName)
    {
        if (!users.Contains(oldName) || users.Contains(newName) || string.IsNullOrEmpty(newName))
            return;

        int index = users.IndexOf(oldName);
        users[index] = newName;

        SaveUsers();

        LeaderboardManager.Instance.RenameUserInScores(oldName, newName);

        TransferScores(oldName, newName);

        if (GetCurrentUser() == oldName)
            SetCurrentUser(newName);

        OnUserChanged?.Invoke();
    }

    void TransferScores(string oldName, string newName)
    {
        for (int i = 1; i <= 3; i++)
        {
            string oldKey = "HighScore_Level_" + i + "_" + oldName;
            string newKey = "HighScore_Level_" + i + "_" + newName;

            int score = PlayerPrefs.GetInt(oldKey, 0);
            PlayerPrefs.SetInt(newKey, score);
        }

        string oldEndless = "HighScore_Endless_" + oldName;
        string newEndless = "HighScore_Endless_" + newName;

        PlayerPrefs.SetInt(newEndless, PlayerPrefs.GetInt(oldEndless, 0));
    }

    public void SetCurrentUser(string username)
{
    PlayerPrefs.SetString("CurrentUser", username);
    OnUserChanged?.Invoke(); 
}

    public string GetCurrentUser()
    {
        return PlayerPrefs.GetString("CurrentUser", "Guest");
    }
}