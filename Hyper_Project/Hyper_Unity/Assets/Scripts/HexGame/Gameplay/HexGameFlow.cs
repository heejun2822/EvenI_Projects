using System;

/// <summary>Owns the game-flow transitions for one running game session.</summary>
public sealed class HexGameFlow : IDisposable
{
    private readonly HexGameSession session;
    private readonly HexStageController stageController;
    private readonly HexPlayerController playerController;

    public HexGameFlow(HexGameSession session, HexStageController stageController, HexPlayerController playerController)
    {
        this.session = session;
        this.stageController = stageController;
        this.playerController = playerController;
        EventBus<PlayerMovedEvent>.Subscribe(HandlePlayerMove);
        EventBus<FormulaSelectedEvent>.Subscribe(UseStoredFormula);
        EventBus<ReturnToTitleRequestedEvent>.Subscribe(ReturnToTitle);
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
            PublishHealth();
        }

        PublishTimer();
        if (session.State.Health <= 0f)
        {
            EndWithGameOver();
        }
    }

    public void Dispose()
    {
        EventBus<PlayerMovedEvent>.Unsubscribe(HandlePlayerMove);
        EventBus<FormulaSelectedEvent>.Unsubscribe(UseStoredFormula);
        EventBus<ReturnToTitleRequestedEvent>.Unsubscribe(ReturnToTitle);
    }

    private void RestartGame()
    {
        session.StartGame();
        EventBus<GameResultChangedEvent>.Publish(new GameResultChangedEvent(HexGameResult.None));
        LoadCurrentStage();
    }

    private void ReturnToTitle(ReturnToTitleRequestedEvent payload)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
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
        PublishState(stage);
        PublishTimer();
    }

    private void HandlePlayerMove(PlayerMovedEvent payload)
    {
        HexMoveResult move = payload.Move;
        if (move.ReachedGoal)
        {
            CompleteCurrentStage();
            return;
        }

        ApplyMoveReward(move);
        session.StartMoveTimer();
        PublishTimer();
    }

    private void ApplyMoveReward(HexMoveResult move)
    {
        if (move.HasFormula)
        {
            if (!session.State.Acquire(move.Formula))
            {
                PublishScore();
            }

            PublishFormulas();
            return;
        }

        if (move.HasHealth)
        {
            session.RestoreHealth(move.HealthAmount);
            PublishHealth();
        }
    }

    private void UseStoredFormula(FormulaSelectedEvent payload)
    {
        if (!playerController.IsInputEnabled || !session.State.TryApplyStoredFormula(payload.Index))
        {
            return;
        }

        PublishScore();
        PublishFormulas();
    }

    private void CompleteCurrentStage()
    {
        playerController.IsInputEnabled = false;
        StageCompletion completion = session.CompleteStage();
        PublishHealth();
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
        EventBus<TimerStoppedEvent>.Publish(new TimerStoppedEvent());
        EventBus<GameResultChangedEvent>.Publish(new GameResultChangedEvent(HexGameResult.GameOver));
    }

    private void EndWithClear()
    {
        playerController.IsInputEnabled = false;
        session.StopMoveTimer();
        EventBus<TimerStoppedEvent>.Publish(new TimerStoppedEvent());
        EventBus<GameResultChangedEvent>.Publish(new GameResultChangedEvent(HexGameResult.Cleared));
    }

    private void PublishState(StageData stage)
    {
        PublishScore();
        PublishHealth();
        EventBus<GoalChangedEvent>.Publish(new GoalChangedEvent(stage.GoalScore));
        PublishFormulas();
    }

    private void PublishScore() => EventBus<ScoreChangedEvent>.Publish(new ScoreChangedEvent(session.State.Score));
    private void PublishHealth() => EventBus<HealthChangedEvent>.Publish(new HealthChangedEvent(session.State.Health));
    private void PublishFormulas() => EventBus<FormulaInventoryChangedEvent>.Publish(
        new FormulaInventoryChangedEvent(session.State.StoredFormulas));
    private void PublishTimer() => EventBus<TimerChangedEvent>.Publish(new TimerChangedEvent(session.RemainingMoveTime));
}
