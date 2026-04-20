using UnityEngine;
using UnityEngine.UI;

public class SkinSelectorUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button[] skinButtons;

    [Header("Highlight Colors")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.green;

    [Header("Panel")]
    public GameObject panelToClose;

    void OnEnable()
    {
        UpdateHighlight();
    }

    public void SelectSkin(int index)
    {
        SkinManager.Instance.SetSkin(index);

        UpdateHighlight();

        // Auto close panel
        if (panelToClose != null)
            panelToClose.SetActive(false);
    }

    void UpdateHighlight()
    {
        int current = PlayerPrefs.GetInt("SelectedSkin", 0);

        for (int i = 0; i < skinButtons.Length; i++)
        {
            ColorBlock cb = skinButtons[i].colors;

            if (i == current)
                cb.normalColor = selectedColor;
            else
                cb.normalColor = normalColor;

            skinButtons[i].colors = cb;
        }
    }
}