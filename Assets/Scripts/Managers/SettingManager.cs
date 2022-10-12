using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(EPlayerPrefs.sfxVolume.ToString()))
        {
            PlayerPrefs.SetFloat(EPlayerPrefs.sfxVolume.ToString(), 1);
        }
        if (!PlayerPrefs.HasKey(EPlayerPrefs.musicVolume.ToString()))
        {
            PlayerPrefs.SetFloat(EPlayerPrefs.musicVolume.ToString(), .5f);
        }
    }

    private void Start()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(EPlayerPrefs.sfxVolume.ToString());
        musicSlider.value = PlayerPrefs.GetFloat(EPlayerPrefs.musicVolume.ToString());
        SFXChanged();
        MusicChanged();
    }

    public void SFXChanged()
    {
        PlayerPrefs.SetFloat(EPlayerPrefs.sfxVolume.ToString(), sfxSlider.value);
        AudioManager.instance.SFXVolumeChanged(sfxSlider.value);
    }

    public void MusicChanged()
    {
        PlayerPrefs.SetFloat(EPlayerPrefs.musicVolume.ToString(), musicSlider.value);
        AudioManager.instance.MusicVolumeChanged(musicSlider.value);
    }
}