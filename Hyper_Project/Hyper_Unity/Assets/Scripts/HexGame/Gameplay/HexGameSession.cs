using UnityEngine;

/// <summary>Owns persistent game state, stage progression, and move-timer rules.</summary>
public sealed class HexGameSession
{
    private readonly GameSettings settings;
    private readonly MoveTimer moveTimer = new();

    public GameState State { get; } = new();
    public StageData CurrentStage => settings.Stages[State.StageIndex];
    public float RemainingMoveTime => moveTimer.Remaining;

    public HexGameSession(GameSettings settings)
    {
        this.settings = settings;
    }

    public void StartGame()
    {
        State.StartGame(settings);
    }

    public void StartMoveTimer()
    {
        moveTimer.Start(settings.MoveTimeLimit);
    }

    public void StopMoveTimer()
    {
        moveTimer.Stop();
    }

    public bool Tick(float deltaTime)
    {
        if (!moveTimer.Tick(deltaTime))
        {
            return false;
        }

        State.TakeDamage(deltaTime * settings.OvertimeHealthLossPerSecond);
        return true;
    }

    public void RestoreHealth(int amount)
    {
        State.RestoreHealth(amount, settings.InitialHealth);
    }

    public StageCompletion CompleteStage()
    {
        StopMoveTimer();
        State.TakeGoalDamage(CurrentStage.GoalScore);
        if (State.Health <= 0f)
        {
            return StageCompletion.GameOver;
        }

        State.AdvanceStage();
        if (State.StageIndex >= settings.Stages.Count)
        {
            return StageCompletion.Cleared;
        }

        State.ResetStage(settings.InitialScore);
        return StageCompletion.NextStage;
    }
}

public enum StageCompletion
{
    NextStage,
    GameOver,
    Cleared
}
