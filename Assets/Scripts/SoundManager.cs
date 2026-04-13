using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;     // one-shot sounds
    public AudioSource loopSource;    // looping sounds (running)

    [Header("Clips")]
    public AudioClip runClip;
    public AudioClip jumpClip;
    public AudioClip hitClip;
    public AudioClip buttonClip;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ========================
    // PLAY METHODS
    // ========================

    public void PlayJump()
    {
        sfxSource.PlayOneShot(jumpClip);
    }

    public void PlayHit()
    {
        sfxSource.PlayOneShot(hitClip);
    }

    public void PlayButton()
    {
        sfxSource.PlayOneShot(buttonClip);
    }

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
}