using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserButton : MonoBehaviour
{
    public string username;
    public UserUIManager manager;

    public TextMeshProUGUI label;
    public Image background;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.AddListener(HandleClick);
    }

    void HandleClick()
    {
        manager.SelectUser(username);
    }

    public void SetText(string text)
    {
        label.text = text;
    }

    public void Highlight(bool active)
    {
        if (background != null)
        {
            background.color = active ? Color.green : Color.white;
        }
    }
}