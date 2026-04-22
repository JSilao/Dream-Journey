using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;     // one-shot SFX
    public AudioSource bgmSource;     // background music
    public AudioSource loopSource;    // looping sounds (running)

    [Header("Volume Settings")]
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider loopSlider;

    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float loopVolume = 1f;

    [Header("Player SFX")]
    public AudioClip runClip;
    public AudioClip jumpClip;

    [Header("Hit SFX")]
    public AudioClip hitObstacleClip;
    public AudioClip hitSpiritClip;
    public AudioClip hitAirObstacleClip;
    public AudioClip hitMonsterClip;

    [Header("UI SFX")]
    public AudioClip buttonClip;

    [Header("Background Music")]
    public AudioClip bgmClip;
    public AudioClip menuBGMClip;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadVolume();
        ApplyVolume();
        InitializeSliders();
        PlayMenuBGM();
    }

    // =======================
    // SLIDER SETUP
    // =======================
    void InitializeSliders()
    {
        if (bgmSlider != null)
        {
            bgmSlider.minValue = 0f;
            bgmSlider.maxValue = 1f;
            bgmSlider.onValueChanged.RemoveAllListeners();
            bgmSlider.value = bgmVolume;
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.minValue = 0f;
            sfxSlider.maxValue = 1f;
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (loopSlider != null)
        {
            loopSlider.minValue = 0f;
            loopSlider.maxValue = 1f;
            loopSlider.onValueChanged.RemoveAllListeners();
            loopSlider.value = loopVolume;
            loopSlider.onValueChanged.AddListener(SetLoopVolume);
        }
    }

    void ApplyVolume()
    {
        if (bgmSource != null)
            bgmSource.volume = bgmVolume;

        if (loopSource != null)
            loopSource.volume = loopVolume;
    }

    // =======================
    // VOLUME CONTROL
    // =======================
    public void SetBGMVolume(float value)
    {
        bgmVolume = value;

        if (bgmSource != null)
            bgmSource.volume = value;

        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void SetLoopVolume(float value)
    {
        loopVolume = value;

        if (loopSource != null)
            loopSource.volume = value;

        PlayerPrefs.SetFloat("LoopVolume", value);
    }

    void LoadVolume()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        loopVolume = PlayerPrefs.GetFloat("LoopVolume", 1f);
    }

    // =======================
    // BACKGROUND MUSIC
    // =======================
    public void PlayMenuBGM()
    {
        if (bgmSource.clip == menuBGMClip && bgmSource.isPlaying)
            return;

        bgmSource.clip = menuBGMClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopMenuBGM()
    {
        bgmSource.Stop();
    }

    public void PlayBGM()
    {
        if (bgmSource.clip == bgmClip && bgmSource.isPlaying)
            return;

        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void repeatBGM()
    {
        if (bgmSource.clip == bgmClip)
        {
            bgmSource.Stop();
            bgmSource.Play();
        }
    }

    // =======================
    // RUN LOOP
    // =======================
    public void StartRun()
    {
        if (loopSource.clip == runClip && loopSource.isPlaying)
            return;

        loopSource.clip = runClip;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void StopRun()
    {
        if (loopSource.isPlaying)
            loopSource.Stop();
    }

    // =======================
    // PLAYER SFX
    // =======================
    public void PlayJump()
    {
        PlaySFX(jumpClip);
    }

    // =======================
    // HIT EVENTS
    // =======================
    public void HitObstacle() => PlaySFX(hitObstacleClip);
    public void HitSpirit() => PlaySFX(hitSpiritClip);
    public void HitAirObstacle() => PlaySFX(hitAirObstacleClip);
    public void HitMonster() => PlaySFX(hitMonsterClip);

    // =======================
    // UI
    // =======================
    public void PlayButton()
    {
        PlaySFX(buttonClip);
    }

    // =======================
    // CORE SFX SYSTEM (FIXED)
    // =======================
    void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.pitch = Random.Range(0.95f, 1.05f);

        // FIX: volume now properly applied
        sfxSource.PlayOneShot(clip, sfxVolume);

        sfxSource.pitch = 1f;
    }
}