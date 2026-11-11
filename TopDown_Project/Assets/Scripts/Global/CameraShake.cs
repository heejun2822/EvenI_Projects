using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin perlin;
    private float shakeTimeRemaining;

    private void Awake()
    {
        perlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float duration, float amplitude, float frequency)
    {
        if (shakeTimeRemaining > duration)
            return;

        shakeTimeRemaining = duration;

        perlin.AmplitudeGain = amplitude;
        perlin.FrequencyGain = frequency;
    }

    private void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;
            if (shakeTimeRemaining <= 0f)
            {
                StopShake();
            }
        }
    }

    public void StopShake()
    {
        shakeTimeRemaining = 0f;
        perlin.AmplitudeGain = 0f;
        perlin.FrequencyGain = 0f;
    }
}
