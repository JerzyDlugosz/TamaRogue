using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource effectsAudioSoruce;

    private AudioClip currentMusicAudioClip;

    /// <summary>
    /// Zone music. Lists can contain loop or start + loop
    /// </summary>
    [SerializeField]
    private List<AudioClip> titleAudio;
    [SerializeField]
    private List<AudioClip> gameAudio;
    [SerializeField]
    private List<AudioClip> shopAudio;
    [SerializeField]
    private List<AudioClip> deathAudio;

    [Range(0, 1)]
    public float mainVolume;
    [Range(0, 1)]
    public float musicVolume;
    [Range(0, 1)]
    public float effectsVolume;


    private void Start()
    {
        mainVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        effectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 0.5f);
    }

    public void OnMapChange(Zone zone)
    {


        switch (zone)
        {
            case Zone.TitleScreen:
                PlayAudioFromList(titleAudio);
                break;
            case Zone.Game:
                PlayAudioFromList(gameAudio);
                break;
            case Zone.Shop:
                PlayAudioFromList(shopAudio);
                break;
            case Zone.Death:
                PlayAudioFromList(deathAudio);
                break;
            default:
                break;
        }
    }

    private void PlayAudioFromList(List<AudioClip> audioClips)
    {
        foreach (var item in audioClips)
        {
            if (currentMusicAudioClip == item)
                return;
        }

        currentMusicAudioClip = audioClips[0];
        if (audioClips.Count > 1)
        {
            musicAudioSource.Stop();
            musicAudioSource.PlayOneShot(audioClips[0]);

            musicAudioSource.clip = audioClips[1];
            musicAudioSource.PlayScheduled(AudioSettings.dspTime + audioClips[0].length);
        }
        else
        {
            PlayAudio(audioClips[0]);
        }
    }

    private void PlayAudio(AudioClip audioClip)
    {
        Debug.Log($"Playing {audioClip.name}");
        musicAudioSource.Stop();
        musicAudioSource.clip = audioClip;
        musicAudioSource.Play();
    }

    public void RemoveAudio()
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = null;
        currentMusicAudioClip = null;
    }

    public void ChangeAudio(AudioClip audioClip)
    {
        PlayAudio(audioClip);
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            Debug.LogWarning("No audio clip to play");
            return;
        }
        Debug.Log($"Playing: {audioClip.name}");
        effectsAudioSoruce.PlayOneShot(audioClip, effectsVolume);
    }

    public void PlaySoundEffectWait(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            Debug.LogWarning("No audio clip to play");
            return;
        }
        if(!effectsAudioSoruce.isPlaying)
        {
            Debug.Log($"Playing: {audioClip.name}");
            effectsAudioSoruce.PlayOneShot(audioClip, effectsVolume);
        }
    }

    private void Update()
    {
        AudioListener.volume = mainVolume;
        musicAudioSource.volume = musicVolume;
        effectsAudioSoruce.volume = effectsVolume;
    }
}

public enum Zone
{
    TitleScreen = 1,
    Game = 2,
    Shop = 3,
    Death = 4
}
