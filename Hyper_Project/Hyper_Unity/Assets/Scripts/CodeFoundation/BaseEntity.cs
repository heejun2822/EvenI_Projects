using UnityEngine;

/// <summary>Defers an actor's execution until its scene director finishes initialization.</summary>
public abstract class BaseEntity : MonoBehaviour
{
    protected bool IsInitialized { get; private set; }

    protected virtual void Awake()
    {
        enabled = false;
    }

    protected void CompleteInitialization()
    {
        if (IsInitialized)
        {
            return;
        }

        IsInitialized = true;
        enabled = true;
    }
}
