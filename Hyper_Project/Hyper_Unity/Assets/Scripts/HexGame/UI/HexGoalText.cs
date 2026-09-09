using TMPro;
using UnityEngine;

/// <summary>Displays the current stage goal for its own text object.</summary>
public sealed class HexGoalText : MonoBehaviour
{
    private TextMeshProUGUI goalText;

    private void Awake() => goalText = GetComponent<TextMeshProUGUI>();
    private void OnEnable() => EventBus<GoalChangedEvent>.Subscribe(Refresh);
    private void OnDisable() => EventBus<GoalChangedEvent>.Unsubscribe(Refresh);
    private void Refresh(GoalChangedEvent payload) => goalText.text = payload.Goal.ToString();
}
