using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("Music UI")]
    public Slider musicSlider;
    public Toggle musicToggle;
    public Image musicIconOn;
    public Image musicIconOff;

    [Header("SFX UI")]
    public Slider sfxSlider;
    public Toggle sfxToggle;
    public Image sfxIconOn;
    public Image sfxIconOff;

    private void Start()
    {
        // Khởi tạo giá trị từ AudioManager
        if (AudioManager.Instance != null)
        {
            musicSlider.value = AudioManager.Instance.GetMusicVolume();
            sfxSlider.value = AudioManager.Instance.GetSFXVolume();

            musicToggle.isOn = AudioManager.Instance.IsMusicMuted();
            sfxToggle.isOn = AudioManager.Instance.IsSFXMuted();

            UpdateMusicIcon(musicToggle.isOn);
            UpdateSFXIcon(sfxToggle.isOn);
        }

        // Gắn sự kiện
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        musicToggle.onValueChanged.AddListener(OnMusicMuteToggled);
        sfxToggle.onValueChanged.AddListener(OnSFXMuteToggled);
    }

    private void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance?.SetMusicVolume(value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance?.SetSFXVolume(value);
    }

    private void OnMusicMuteToggled(bool isMute)
    {
        AudioManager.Instance?.ToggleMusicMute(isMute);
        UpdateMusicIcon(isMute);
    }

    private void OnSFXMuteToggled(bool isMute)
    {
        AudioManager.Instance?.ToggleSFXMute(isMute);
        UpdateSFXIcon(isMute);
    }

    // ---------------- ICON UPDATE ----------------
    private void UpdateMusicIcon(bool isMuted)
    {
        if (musicIconOn != null) musicIconOn.gameObject.SetActive(!isMuted);
        if (musicIconOff != null) musicIconOff.gameObject.SetActive(isMuted);
        musicSlider.interactable = !isMuted;                
    }

    private void UpdateSFXIcon(bool isMuted)
    {
        if (sfxIconOn != null) sfxIconOn.gameObject.SetActive(!isMuted);
        if (sfxIconOff != null) sfxIconOff.gameObject.SetActive(isMuted);
        sfxSlider.interactable = !isMuted;
    }
}
