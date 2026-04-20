using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    private List<ScoreEntry> allScores = new List<ScoreEntry>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadScores();
    }

    // =========================
    // SAVE / LOAD
    // =========================
    void LoadScores()
    {
        string json = PlayerPrefs.GetString("ALL_SCORES", "");

        if (!string.IsNullOrEmpty(json))
        {
            ScoreList wrapper = JsonUtility.FromJson<ScoreList>(json);
            allScores = wrapper.list;
        }
    }

    void SaveScores()
    {
        ScoreList wrapper = new ScoreList();
        wrapper.list = allScores;

        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString("ALL_SCORES", json);
        PlayerPrefs.Save();
    }

    [System.Serializable]
    class ScoreList
    {
        public List<ScoreEntry> list = new List<ScoreEntry>();
    }

    // =========================
    // ADD SCORE
    // =========================
    public void AddScore(string user, int score)
    {
        // Reset all previous latest flags
        foreach (var s in allScores)
            s.isLatest = false;

        // Add new score as latest
        allScores.Add(new ScoreEntry(user, score));

        SaveScores();
    }

    // =========================
    // GLOBAL TOP 20
    // =========================
    public List<ScoreEntry> GetGlobalTop20()
    {
        return allScores
            .OrderByDescending(s => s.score)
            .Take(10)
            .ToList();
    }

    // =========================
    // USER TOP 20
    // =========================
    public List<ScoreEntry> GetUserTop20(string user)
    {
        return allScores
            .Where(s => s.username == user)
            .OrderByDescending(s => s.score)
            .Take(10)
            .ToList();
    }

    public void RenameUserInScores(string oldName, string newName)
{
    for (int i = 0; i < allScores.Count; i++)
    {
        if (allScores[i].username == oldName)
        {
            allScores[i].username = newName;
        }
    }

    SaveScores();
}
}