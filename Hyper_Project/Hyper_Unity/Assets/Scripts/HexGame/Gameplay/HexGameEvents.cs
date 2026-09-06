using System.Collections.Generic;
using UnityEngine;

public readonly struct PointerTappedEvent
{
    public Vector2 ScreenPosition { get; }
    public PointerTappedEvent(Vector2 screenPosition) => ScreenPosition = screenPosition;
}

public readonly struct PointerDraggedEvent
{
    public Vector2 Delta { get; }
    public PointerDraggedEvent(Vector2 delta) => Delta = delta;
}

public readonly struct PointerZoomedEvent
{
    public Vector2 Delta { get; }
    public PointerZoomedEvent(Vector2 delta) => Delta = delta;
}

public readonly struct PlayerMovedEvent
{
    public HexMoveResult Move { get; }
    public PlayerMovedEvent(HexMoveResult move) => Move = move;
}

public readonly struct FormulaSelectedEvent
{
    public int Index { get; }
    public FormulaSelectedEvent(int index) => Index = index;
}

public readonly struct ReturnToTitleRequestedEvent { }
public readonly struct TimerStoppedEvent { }

public readonly struct ScoreChangedEvent
{
    public int Score { get; }
    public ScoreChangedEvent(int score) => Score = score;
}

public readonly struct HealthChangedEvent
{
    public float Health { get; }
    public HealthChangedEvent(float health) => Health = health;
}

public readonly struct GoalChangedEvent
{
    public int Goal { get; }
    public GoalChangedEvent(int goal) => Goal = goal;
}

public readonly struct FormulaInventoryChangedEvent
{
    public IReadOnlyList<FormulaTileData> Formulas { get; }
    public FormulaInventoryChangedEvent(IReadOnlyList<FormulaTileData> formulas) => Formulas = formulas;
}

public readonly struct TimerChangedEvent
{
    public float Remaining { get; }
    public TimerChangedEvent(float remaining) => Remaining = remaining;
}

public enum HexGameResult
{
    None,
    GameOver,
    Cleared
}

public readonly struct GameResultChangedEvent
{
    public HexGameResult Result { get; }
    public GameResultChangedEvent(HexGameResult result) => Result = result;
}
