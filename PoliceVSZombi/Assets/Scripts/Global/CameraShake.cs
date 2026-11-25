using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    CinemachineVirtualCamera virutCamera;
    CinemachineBasicMultiChannelPerlin multiChannelPerlin;
    float shakeDuration;
    float shakeTimeRemaining;

    private void Awake()
    {
        virutCamera = GetComponent<CinemachineVirtualCamera>();
        multiChannelPerlin = virutCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float duration, float amplitude, float frequency)
    {
        if (shakeTimeRemaining > shakeDuration)
            return;

        shakeDuration = duration;
        shakeTimeRemaining = duration;

        multiChannelPerlin.m_AmplitudeGain = amplitude;
        multiChannelPerlin.m_FrequencyGain = frequency;
    }

    private void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;
            if(shakeTimeRemaining <= 0)
            {
                StopShake();
            }
        }
    }

    public void StopShake()
    {
        multiChannelPerlin.m_AmplitudeGain = 0f;
        multiChannelPerlin.m_FrequencyGain = 0f;
    }
}
