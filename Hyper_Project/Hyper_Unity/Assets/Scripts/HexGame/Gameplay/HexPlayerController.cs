using UnityEngine;

/// <summary>Converts pointer taps into movement attempts for the active stage.</summary>
public sealed class HexPlayerController : MonoBehaviour
{
    [SerializeField] private HexStageController stageController;

    public bool IsInputEnabled { get; set; }
    public bool IsConfigured => stageController != null;

    private void OnEnable()
    {
        EventBus<PointerTappedEvent>.Subscribe(TryMoveAtScreenPosition);
    }

    private void OnDisable()
    {
        EventBus<PointerTappedEvent>.Unsubscribe(TryMoveAtScreenPosition);
    }

    public void SetWorldPosition(Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }

    private void TryMoveAtScreenPosition(PointerTappedEvent payload)
    {
        if (!IsInputEnabled)
        {
            return;
        }

        if (stageController.TryMoveAtScreenPosition(payload.ScreenPosition, out HexMoveResult move))
        {
            EventBus<PlayerMovedEvent>.Publish(new PlayerMovedEvent(move));
        }
    }
}
