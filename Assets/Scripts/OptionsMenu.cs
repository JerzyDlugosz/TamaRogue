using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider mainVolumeSlider;
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private Slider effectsVolumeSlider;

    public AudioClip testSoundEffect;

    private void Start()
    {
        SetUIValues();

        mainVolumeSlider.onValueChanged.AddListener(delegate {
            OnMainVolumeSliderChange(mainVolumeSlider);
        });

        musicVolumeSlider.onValueChanged.AddListener(delegate {
            OnMusicVolumeSliderChange(musicVolumeSlider);
        });

        effectsVolumeSlider.onValueChanged.AddListener(delegate {
            OnEffectsVolumeSliderChange(effectsVolumeSlider);
        });

    }

    private void SetUIValues()
    {
        float mainVolume = PlayerPrefs.GetFloat("MainVolume", 0.5f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float effectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 0.5f);

        mainVolumeSlider.value = mainVolume;
        musicVolumeSlider.value = musicVolume;
        effectsVolumeSlider.value = effectsVolume;
    }


    public void OnMainVolumeSliderChange(Slider slider)
    {
        GameStateManager.instance.audioManager.mainVolume = slider.value;
        PlayerPrefs.SetFloat("MusicVolume", slider.value);
    }

    public void OnMusicVolumeSliderChange(Slider slider)
    {
        GameStateManager.instance.audioManager.musicVolume = slider.value;
        PlayerPrefs.SetFloat("MusicVolume", slider.value);
    }

    public void OnEffectsVolumeSliderChange(Slider slider)
    {
        GameStateManager.instance.audioManager.effectsVolume = slider.value;
        PlayerPrefs.SetFloat("EffectsVolume", slider.value);
        GameStateManager.instance.audioManager.PlaySoundEffectWait(testSoundEffect);
    }
}
