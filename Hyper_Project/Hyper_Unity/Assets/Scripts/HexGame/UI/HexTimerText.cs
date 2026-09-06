using TMPro;
using UnityEngine;

/// <summary>Displays and animates the move timer for its own text object.</summary>
public sealed class HexTimerText : MonoBehaviour
{
    [SerializeField] private GameObject timerObject;

    private const float WarningSeconds = 3f;
    private const float ShakeFrequency = 30f;
    private const float ShakeDistance = 8f;

    private TextMeshProUGUI timerText;
    private Vector2 basePosition;

    private void Awake()
    {
        timerText = timerObject.GetComponent<TextMeshProUGUI>();
        basePosition = timerText.rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        EventBus<TimerChangedEvent>.Subscribe(Refresh);
        EventBus<TimerStoppedEvent>.Subscribe(Hide);
    }

    private void OnDisable()
    {
        EventBus<TimerChangedEvent>.Unsubscribe(Refresh);
        EventBus<TimerStoppedEvent>.Unsubscribe(Hide);
    }

    private void Refresh(TimerChangedEvent payload)
    {
        bool isWarning = payload.Remaining <= WarningSeconds;
        timerObject.SetActive(true);
        timerText.text = Mathf.CeilToInt(Mathf.Max(0f, payload.Remaining)).ToString();
        timerText.color = isWarning ? new Color(1f, .2f, .2f) : Color.black;
        timerText.rectTransform.anchoredPosition = basePosition + (isWarning
            ? new Vector2(Mathf.Sin(Time.unscaledTime * ShakeFrequency) * ShakeDistance, 0f)
            : Vector2.zero);
    }

    private void Hide(TimerStoppedEvent payload)
    {
        timerObject.SetActive(false);
        timerText.rectTransform.anchoredPosition = basePosition;
    }
}
