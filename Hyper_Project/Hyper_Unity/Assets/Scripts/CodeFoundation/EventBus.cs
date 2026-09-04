using System;

/// <summary>Broadcasts value-type facts to independent subscribers.</summary>
public static class EventBus<T> where T : struct
{
    private static Action<T> listeners;

    public static void Subscribe(Action<T> listener)
    {
        if (listener == null)
        {
            throw new ArgumentNullException(nameof(listener));
        }

        listeners += listener;
    }

    public static void Unsubscribe(Action<T> listener)
    {
        if (listener == null)
        {
            throw new ArgumentNullException(nameof(listener));
        }

        listeners -= listener;
    }

    public static void Publish(T payload)
    {
        listeners?.Invoke(payload);
    }
}
