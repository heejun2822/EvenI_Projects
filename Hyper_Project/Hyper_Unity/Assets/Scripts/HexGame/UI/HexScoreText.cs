using TMPro;
using UnityEngine;

/// <summary>Displays score updates for its own text object.</summary>
public sealed class HexScoreText : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void Awake() => scoreText = GetComponent<TextMeshProUGUI>();
    private void OnEnable() => EventBus<ScoreChangedEvent>.Subscribe(Refresh);
    private void OnDisable() => EventBus<ScoreChangedEvent>.Unsubscribe(Refresh);
    private void Refresh(ScoreChangedEvent payload) => scoreText.text = $"Score: {payload.Score}";
}
