using System;

/// <summary>Owns the game-flow transitions for one running game session.</summary>
public sealed class HexGameFlow : IDisposable
{
    private readonly HexGameSession session;
    private readonly HexStageController stageController;
    private readonly HexPlayerController playerController;
    private readonly HexGameUi ui;

    public HexGameFlow(HexGameSession session, HexStageController stageController, HexPlayerController playerController, HexGameUi ui)
    {
        this.session = session;
        this.stageController = stageController;
        this.playerController = playerController;
        this.ui = ui;

        playerController.Moved += HandlePlayerMove;
        ui.Bind(UseStoredFormula, RestartGame);
    }

    public void Start()
    {
        RestartGame();
    }

    public void Tick(float deltaTime)
    {
        if (!playerController.IsInputEnabled)
        {
            return;
        }

        if (session.Tick(deltaTime))
        {
            ui.RefreshHealth(session.State);
        }

        ui.RefreshTimer(session.RemainingMoveTime);
        if (session.State.Health <= 0f)
        {
            EndWithGameOver();
        }
    }

    public void Dispose()
    {
        playerController.Moved -= HandlePlayerMove;
        playerController.Dispose();
    }

    private void RestartGame()
    {
        session.StartGame();
        ui.HideResultPanels();
        LoadCurrentStage();
    }

    private void LoadCurrentStage()
    {
        StageData stage = session.CurrentStage;
        if (!stage.Validate(out string error))
        {
            UnityEngine.Debug.LogError($"Stage '{stage.name}' is invalid: {error}", stage);
            playerController.IsInputEnabled = false;
            return;
        }

        stageController.Load(stage);
        playerController.IsInputEnabled = true;
        session.StartMoveTimer();
        ui.RefreshStage(session.State, stage);
        ui.RefreshTimer(session.RemainingMoveTime);
    }

    private void HandlePlayerMove(HexMoveResult move)
    {
        if (move.ReachedGoal)
        {
            CompleteCurrentStage();
            return;
        }

        ApplyMoveReward(move);
        session.StartMoveTimer();
        ui.RefreshTimer(session.RemainingMoveTime);
    }

    private void ApplyMoveReward(HexMoveResult move)
    {
        if (move.HasFormula)
        {
            if (!session.State.Acquire(move.Formula))
            {
                ui.RefreshScore(session.State);
            }

            ui.RefreshFormulas(session.State);
            return;
        }

        if (move.HasHealth)
        {
            session.RestoreHealth(move.HealthAmount);
            ui.RefreshHealth(session.State);
        }
    }

    private void UseStoredFormula(int index)
    {
        if (!playerController.IsInputEnabled || !session.State.TryApplyStoredFormula(index))
        {
            return;
        }

        ui.RefreshScore(session.State);
        ui.RefreshFormulas(session.State);
    }

    private void CompleteCurrentStage()
    {
        playerController.IsInputEnabled = false;
        StageCompletion completion = session.CompleteStage();
        ui.RefreshHealth(session.State);
        switch (completion)
        {
            case StageCompletion.NextStage:
                LoadCurrentStage();
                break;
            case StageCompletion.GameOver:
                EndWithGameOver();
                break;
            case StageCompletion.Cleared:
                EndWithClear();
                break;
        }
    }

    private void EndWithGameOver()
    {
        playerController.IsInputEnabled = false;
        session.StopMoveTimer();
        ui.StopTimer();
        ui.ShowGameOver();
    }

    private void EndWithClear()
    {
        playerController.IsInputEnabled = false;
        session.StopMoveTimer();
        ui.StopTimer();
        ui.ShowClear();
    }
}
