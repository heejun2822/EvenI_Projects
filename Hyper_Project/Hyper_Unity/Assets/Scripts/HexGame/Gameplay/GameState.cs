using System;
using System.Collections.Generic;

public sealed class GameState
{
    public const int FormulaCapacity = 3;

    private readonly List<FormulaTileData> storedFormulas = new(FormulaCapacity);

    public int Score { get; private set; }
    public float Health { get; private set; }
    public int StageIndex { get; private set; }
    public IReadOnlyList<FormulaTileData> StoredFormulas => storedFormulas;

    public void StartGame(GameSettings settings)
    {
        Health = settings.InitialHealth;
        StageIndex = 0;
        ResetStage(settings.InitialScore);
    }

    public void ResetStage(int initialScore)
    {
        Score = initialScore;
        storedFormulas.Clear();
    }

    public bool Acquire(FormulaTileData formula)
    {
        if (storedFormulas.Count == FormulaCapacity)
        {
            Score = formula.Apply(Score);
            return false;
        }

        storedFormulas.Add(formula);
        return true;
    }

    public bool TryApplyStoredFormula(int index)
    {
        if (index < 0 || index >= storedFormulas.Count)
        {
            return false;
        }

        Score = storedFormulas[index].Apply(Score);
        storedFormulas.RemoveAt(index);
        return true;
    }

    public void TakeGoalDamage(int targetScore)
    {
        Health -= Math.Abs(Score - targetScore);
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
    }

    public void RestoreHealth(float amount, float maximumHealth)
    {
        Health = Math.Min(maximumHealth, Health + amount);
    }

    public void AdvanceStage()
    {
        StageIndex++;
    }
}
