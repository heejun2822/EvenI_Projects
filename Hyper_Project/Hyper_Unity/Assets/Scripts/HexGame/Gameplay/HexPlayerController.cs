using System;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>Converts pointer taps into movement attempts for the active stage.</summary>
public sealed class HexPlayerController : IDisposable
{
    private readonly Camera mainCamera;
    private readonly Tilemap tilemap;
    private readonly HexStageController stageController;
    private PointerGestureInput gestureInput;

    public bool IsInputEnabled { get; set; }
    public event Action<HexMoveResult> Moved;

    public HexPlayerController(Camera mainCamera, Tilemap tilemap, HexStageController stageController)
    {
        this.mainCamera = mainCamera;
        this.tilemap = tilemap;
        this.stageController = stageController;
    }

    public void Bind(PointerGestureInput input)
    {
        Dispose();
        gestureInput = input;
        gestureInput.Tapped += TryMoveAtScreenPosition;
    }

    public void Dispose()
    {
        if (gestureInput != null)
        {
            gestureInput.Tapped -= TryMoveAtScreenPosition;
            gestureInput = null;
        }
    }

    private void TryMoveAtScreenPosition(Vector2 screenPosition)
    {
        if (!IsInputEnabled)
        {
            return;
        }

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, -mainCamera.transform.position.z));
        Vector3Int target = tilemap.WorldToCell(worldPosition);
        if (stageController.TryMove(target, out HexMoveResult move))
        {
            Moved?.Invoke(move);
        }
    }
}
