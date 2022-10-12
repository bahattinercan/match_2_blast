using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum EGameSoundType
{
    ball,
    bomb,
    boosterAward,
    collectable,
    colorBomb,
    cubePress,
    cubePressError,
    dynamite,
    iceBreak,
    reachedGoal,
    stone,
}

[System.Serializable]
public enum EUISoundType
{
    button,
    buyPopButton,
    coinsPopButton,
    lose,
    popupClose,
    popupCloseButton,
    popupOpen,
    popupOpenButton,
    popupOpenWhoosh,
    rain,
    win,
    winStarPop,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private float musicVolume;
    private float sfxVolume;
    [SerializeField] private AudioSource[] gameSounds;
    [SerializeField] private AudioSource[] uiSounds;
    private AudioSource musicAudioSource;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.Play();
    }

    public void MusicVolumeChanged(float volume)
    {
        musicVolume = volume;
        musicAudioSource.volume = volume;
        PlayerPrefs.SetFloat(EPlayerPrefs.musicVolume.ToString(), musicVolume);
    }

    public void SFXVolumeChanged(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat(EPlayerPrefs.sfxVolume.ToString(), sfxVolume);
        foreach (AudioSource audioSource in gameSounds)
        {
            audioSource.volume = volume;
        }
        foreach (AudioSource audioSource in uiSounds)
        {
            audioSource.volume = volume;
        }
    }

    public void PlaySFX(EUISoundType soundType, bool changePitch = true)
    {
        uiSounds[(int)soundType].Play();
        if (changePitch)
            uiSounds[(int)soundType].pitch = Random.Range(.9f, 1.1f);
        else
            uiSounds[(int)soundType].pitch = 1;
    }

    public void PlaySFX(EGameSoundType soundType, bool changePitch = true)
    {
        gameSounds[(int)soundType].Play();
        if (changePitch)
            gameSounds[(int)soundType].pitch = Random.Range(.9f, 1.1f);
        else
            gameSounds[(int)soundType].pitch = 1;
    }
}