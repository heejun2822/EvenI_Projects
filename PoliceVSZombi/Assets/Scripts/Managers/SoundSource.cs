using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    private AudioSource audioSource;

    public void Play(AudioClip clip, float soundEffectVolume, float soundEffectPichVariance)
    {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();

        CancelInvoke();

        audioSource.clip = clip;
        audioSource.volume = soundEffectVolume;

        audioSource.Play();
        audioSource.pitch = 1f + Random.Range(-soundEffectPichVariance, soundEffectPichVariance);

        Invoke("Disable", clip.length + 2);
    }

    public void Disable()
    {
        audioSource.Stop();
        gameObject.SetActive(false);
    }
}
