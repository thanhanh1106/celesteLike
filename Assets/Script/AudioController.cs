using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    [Header("Main Setting")]
    [Range(0f, 1f)] public float MusicVolume;
    [Range(0f, 1f)] public float SFXVolume;

    public AudioSource MusicAuidoSoure;
    public AudioSource SFXAudioSoure;

    [Header("Game sound anh music")]
    public AudioClip Jump;
    public AudioClip Dash;
    public AudioClip Die;
    public AudioClip BackGroundMusic;

    public void PlaySound(AudioClip sound,AudioSource audioSource = null)
    {
        if (!audioSource) audioSource = SFXAudioSoure;
        if (audioSource) audioSource.PlayOneShot(sound, SFXVolume);
    }
    public void PlaySound(AudioClip[] audioClips,AudioSource audioSource = null)
    {
        if (!audioSource) audioSource = SFXAudioSoure;
        if(audioSource)
        {
            int rand = Random.Range(0,audioClips.Length);
            if (audioClips[rand]) audioSource.PlayOneShot(audioClips[rand], SFXVolume);
        }
    }

    public void PlayMusic(AudioClip audioClip,bool loop = true)
    {
        if (MusicAuidoSoure)
        {
            MusicAuidoSoure.clip = audioClip;
            MusicAuidoSoure.loop = loop;
            MusicAuidoSoure.volume = SFXVolume;
            MusicAuidoSoure.Play();
        }
    }
    public void PlayMusic(AudioClip[] audioClips,bool loop = true)
    {
        if (MusicAuidoSoure)
        {
            int rand = Random.Range(0, audioClips.Length);
            if (audioClips[rand])
            {
                MusicAuidoSoure.clip = audioClips[rand];
                MusicAuidoSoure.loop = loop;
                MusicAuidoSoure.volume = SFXVolume;
                MusicAuidoSoure.Play();
            }
        }
    }
}
