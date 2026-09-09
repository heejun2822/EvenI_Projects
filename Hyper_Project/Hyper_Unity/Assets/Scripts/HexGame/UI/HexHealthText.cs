using TMPro;
using UnityEngine;

/// <summary>Displays health and highlights it while overtime damage is active.</summary>
public sealed class HexHealthText : MonoBehaviour
{
    private TextMeshProUGUI healthText;
    private Color baseColor;

    private void Awake()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        baseColor = healthText.color;
    }

    private void OnEnable()
    {
        EventBus<HealthChangedEvent>.Subscribe(RefreshHealth);
        EventBus<TimerChangedEvent>.Subscribe(RefreshTimerState);
    }

    private void OnDisable()
    {
        EventBus<HealthChangedEvent>.Unsubscribe(RefreshHealth);
        EventBus<TimerChangedEvent>.Unsubscribe(RefreshTimerState);
    }

    private void RefreshHealth(HealthChangedEvent payload) => healthText.text = Mathf.CeilToInt(payload.Health).ToString();
    private void RefreshTimerState(TimerChangedEvent payload) => healthText.color = payload.Remaining <= 0f ? new Color(1f, .2f, .2f) : baseColor;
}
