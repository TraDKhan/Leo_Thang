using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Audio Clips")]
    public AudioClip pickUpCoinClip;
    public AudioClip pickUpItemClip;
    public AudioClip completeLevelClip;
    public AudioClip gameLoseClip;
    public AudioClip moveClip;
    public AudioClip fallClip;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // ------------------ GET VALUE ------------------
    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;
    public bool IsMusicMuted() => musicSource.mute;
    public bool IsSFXMuted() => sfxSource.mute;

    // ------------------ PLAY ------------------
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
    public void PlayPickUpCoin() => PlaySFX(pickUpCoinClip);
    public void PlayPickUpItem() => PlaySFX(pickUpItemClip);
    public void PlayCompleteLevel() => PlaySFX(completeLevelClip);
    public void PlayGameLose() => PlaySFX(gameLoseClip);
    public void PlayMove() => PlaySFX(moveClip);
    public void PlayFall() => PlaySFX(fallClip);

    // ------------------ VOLUME ------------------
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = sfxVolume;
    }

    // ------------------ MUTE ------------------
    public void ToggleMusicMute(bool isMute)
    {
        musicSource.mute = isMute;
    }

    public void ToggleSFXMute(bool isMute)
    {
        sfxSource.mute = isMute;
    }
}
