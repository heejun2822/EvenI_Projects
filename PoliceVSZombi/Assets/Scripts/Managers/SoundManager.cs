using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance;
    [SerializeField][Range(0f, 1f)] private float musicVolume;

    ObjectPool objectPool;

    private AudioSource musicAudioSource;
    public AudioClip musicClip;

    void Awake()
    {
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;

        objectPool = GetComponent<ObjectPool>();
    }

    private void Start()
    {
        ChangeBackGroundMusic(musicClip);
    }

    public static void ChangeBackGroundMusic(AudioClip musicClip)
    {
        Instance.musicAudioSource.Stop();
        Instance.musicAudioSource.clip = musicClip;
        Instance.musicAudioSource.Play();
    }

    public static void PlayClip(AudioClip clip, float volumeMultiplier = 1.0f)
    {
        GameObject go = Instance.objectPool.SpawnFromPool("SoundSource");
        go.SetActive(true);
        SoundSource soundSource = go.GetComponent<SoundSource>();
        soundSource.Play(clip, Instance.soundEffectVolume * volumeMultiplier, Instance.soundEffectPitchVariance);
    }

}
