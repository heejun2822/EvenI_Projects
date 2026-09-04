using System.Threading;
using UnityEngine;

/// <summary>Owns one scene's initialization boundary and cancellation lifetime.</summary>
public abstract class BaseSceneDirector : MonoBehaviour
{
    private CancellationTokenSource sceneCancellation;

    protected bool IsInitialized { get; private set; }
    protected CancellationToken SceneCancellationToken => sceneCancellation.Token;

    protected virtual void Awake()
    {
        sceneCancellation = new CancellationTokenSource();
        if (!GlobalBootstrapper.IsInitialized)
        {
            Debug.LogError("GlobalBootstrapper must finish before a scene director starts.", this);
            enabled = false;
        }
    }

    protected virtual void Start()
    {
        if (!enabled)
        {
            return;
        }

        IsInitialized = InitializeScene();
        if (!IsInitialized)
        {
            enabled = false;
        }
    }

    protected virtual void OnDestroy()
    {
        sceneCancellation?.Cancel();
        sceneCancellation?.Dispose();
    }

    protected abstract bool InitializeScene();
}
