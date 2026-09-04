using UnityEngine;

/// <summary>Establishes the app-wide initialization boundary before any scene is loaded.</summary>
public static class GlobalBootstrapper
{
    public static bool IsInitialized { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        IsInitialized = true;
    }
}
