using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class LeaderboardUI : MonoBehaviour
{
    [Header("Containers (Scroll View Content objects)")]
    public Transform globalContainer;
    public Transform userContainer;

    [Header("Prefab")]
    public GameObject entryPrefab;


    [Header("Layout Settings")]
    public float entryHeight = 90f;

    IEnumerator Start()
{
    yield return new WaitUntil(() =>
        UserManager.Instance != null &&
        LeaderboardManager.Instance != null
    );

    Refresh();
}

    public void Refresh()
    {
        // -------------------------
        // Safety checks
        // -------------------------
        if (globalContainer == null || userContainer == null || entryPrefab == null)
        {
            Debug.LogError("LeaderboardUI: Missing UI references!");
            return;
        }

        if (UserManager.Instance == null || LeaderboardManager.Instance == null)
        {
            Debug.LogError("LeaderboardUI: Missing manager references!");
            return;
        }

        // -------------------------
        // Clear old entries
        // -------------------------
        Clear(globalContainer);
        Clear(userContainer);

        // -------------------------
        // Get data
        // -------------------------
        string currentUser = UserManager.Instance.GetCurrentUser();

        List<ScoreEntry> global = LeaderboardManager.Instance.GetGlobalTop20();
    List<ScoreEntry> user = LeaderboardManager.Instance.GetUserTop20(currentUser);

        // -------------------------
        // Build GLOBAL leaderboard
        // -------------------------
        for (int i = 0; i < global.Count; i++)
        {
            CreateEntry(globalContainer, i, global[i]);
        }

        // adjust scroll content height
        SetContentHeight(globalContainer, global.Count);

        // -------------------------
        // Build USER leaderboard
        // -------------------------
        for (int i = 0; i < user.Count; i++)
        {
            CreateEntry(userContainer, i, user[i]);
        }

        SetContentHeight(userContainer, user.Count);
    }

    void CreateEntry(Transform parent, int index, ScoreEntry entry)
    {
        GameObject go = Instantiate(entryPrefab, parent);

        RectTransform rt = go.GetComponent<RectTransform>();

        // // Force top-down stacking manually
        rt.anchoredPosition = new Vector2(0, -index * entryHeight);

        TextMeshProUGUI txt = go.GetComponentInChildren<TextMeshProUGUI>();

        if (txt == null)
        {
            Debug.LogError("Entry prefab is missing TextMeshProUGUI in children!");
            return;
        }

        string displayDate = string.IsNullOrEmpty(entry.date) ? "Unknown" : entry.date;

        if (entry.isLatest)
        {
            txt.text =
                "<color=red><b>" +
                (index + 1) + ". " + entry.username + " - " + entry.score + " m" +
                "\n<size=70%>" + displayDate + " (LATEST)</size>" +
                "</b></color>";
        }
        else
        {
            txt.text =
                (index + 1) + ". " + entry.username + " - " + entry.score + " m" +
                "\n<size=70%>" + displayDate + "</size>";
        }
    }

    void SetContentHeight(Transform container, int count)
    {
        RectTransform rt = container.GetComponent<RectTransform>();

        if (rt == null) return;

        rt.sizeDelta = new Vector2(
            rt.sizeDelta.x,
            count * entryHeight
        );
    }

    void Clear(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}