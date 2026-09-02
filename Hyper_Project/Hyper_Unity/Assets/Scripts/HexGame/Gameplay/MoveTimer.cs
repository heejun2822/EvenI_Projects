using UnityEngine;

/// <summary>Tracks the interval between moves.</summary>
public sealed class MoveTimer
{
    public float Remaining { get; private set; }
    public bool IsRunning { get; private set; }

    public void Start(float duration)
    {
        Remaining = Mathf.Max(0f, duration);
        IsRunning = true;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public bool Tick(float deltaTime)
    {
        if (!IsRunning)
        {
            return false;
        }

        Remaining -= deltaTime;
        if (Remaining > 0f)
        {
            return false;
        }

        return true;
    }
}
