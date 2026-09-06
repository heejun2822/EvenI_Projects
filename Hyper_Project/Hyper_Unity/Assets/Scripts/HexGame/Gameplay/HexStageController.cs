using UnityEngine;
/// <summary>Loads a stage and maintains its board, player world position, and camera framing.</summary>
public sealed class HexStageController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private HexPlayerController playerController;

    private HexBoardView boardView;
    private CameraPanZoom cameraPanZoom;

    public HexBoardState Board { get; private set; }
    public bool IsConfigured => mainCamera != null && playerController != null && boardView != null && boardView.IsConfigured;

    private void Awake()
    {
        boardView = GetComponent<HexBoardView>();
        cameraPanZoom = mainCamera != null ? mainCamera.GetComponent<CameraPanZoom>() : null;
    }

    public void Load(StageData stage)
    {
        Board = new HexBoardState(stage);
        boardView.Build(Board);
        playerController.SetWorldPosition(boardView.Tilemap.GetCellCenterWorld(Board.CurrentCell));
        ResetCamera();
        RefreshBoard();
    }

    public bool TryMove(Vector3Int target, out HexMoveResult result)
    {
        result = default;
        if (!Board.TryMove(target, out Vector3Int previousCell))
        {
            return false;
        }

        playerController.SetWorldPosition(boardView.Tilemap.GetCellCenterWorld(Board.CurrentCell));
        boardView.RemoveTile(previousCell);
        if (Board.IsGoal(Board.CurrentCell))
        {
            result = HexMoveResult.Goal;
            return true;
        }

        if (Board.TryGetFormula(Board.CurrentCell, out FormulaTileData formula))
        {
            result = HexMoveResult.WithFormula(formula);
        }
        else if (Board.TryGetHealth(Board.CurrentCell, out int healthAmount))
        {
            result = HexMoveResult.WithHealth(healthAmount);
        }

        boardView.RemoveLabel(Board.CurrentCell);
        RefreshBoard();
        return true;
    }

    public bool TryMoveAtScreenPosition(Vector2 screenPosition, out HexMoveResult result)
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, -mainCamera.transform.position.z));
        return TryMove(boardView.Tilemap.WorldToCell(worldPosition), out result);
    }

    private void RefreshBoard()
    {
        Board.RefreshUnavailableCells();
        boardView.Refresh(Board);
    }

    private void ResetCamera()
    {
        if (cameraPanZoom == null)
        {
            return;
        }

        Vector2 panOffset = new(boardView.Tilemap.cellSize.x * 1.5f, boardView.Tilemap.cellSize.y * 1.5f);
        cameraPanZoom.ResetForStage(boardView.CalculateBounds(Board), playerController.transform.position, panOffset);
    }
}

/// <summary>Describes the gameplay content acquired by a successful move.</summary>
public readonly struct HexMoveResult
{
    public static HexMoveResult Goal => new(true, false, default, false, 0);

    public bool ReachedGoal { get; }
    public bool HasFormula { get; }
    public FormulaTileData Formula { get; }
    public bool HasHealth { get; }
    public int HealthAmount { get; }

    private HexMoveResult(bool reachedGoal, bool hasFormula, FormulaTileData formula, bool hasHealth, int healthAmount)
    {
        ReachedGoal = reachedGoal;
        HasFormula = hasFormula;
        Formula = formula;
        HasHealth = hasHealth;
        HealthAmount = healthAmount;
    }

    public static HexMoveResult WithFormula(FormulaTileData formula) => new(false, true, formula, false, 0);
    public static HexMoveResult WithHealth(int healthAmount) => new(false, false, default, true, healthAmount);
}
