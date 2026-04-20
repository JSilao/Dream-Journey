using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    [Header("Skin Data")]
    public RuntimeAnimatorController[] animatorControllers;
    public Sprite[] sprites;

    public int defaultSkinIndex = 0;

    private int currentSkin;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentSkin = PlayerPrefs.GetInt("SelectedSkin", defaultSkinIndex);
    }

    public void SetSkin(int index)
    {
        currentSkin = index;
        PlayerPrefs.SetInt("SelectedSkin", index);
        PlayerPrefs.Save();
    }

    public int GetSkin()
    {
        return currentSkin;
    }

    public RuntimeAnimatorController GetAnimator()
    {
        return animatorControllers[currentSkin];
    }

    public Sprite GetSprite()
    {
        return sprites[currentSkin];
    }
}