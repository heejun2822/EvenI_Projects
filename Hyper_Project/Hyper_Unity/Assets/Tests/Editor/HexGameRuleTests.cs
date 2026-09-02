using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public sealed class HexGameRuleTests
{
    public static void RunAll()
    {
        HexGameRuleTests tests = new();
        tests.FormulaInventory_AppliesOverflowFormulaImmediately();
        tests.StoredFormula_UpdatesScoreAndRemovesOnlyTheSelectedFormula();
        tests.MoveTimer_ReportsExpirationWithoutChangingGameState();
        tests.GetCellsOnAnyPath_ExcludesDeadEndBranches();
        tests.StageValidation_RejectsBoardsWithoutARouteToTheGoal();
    }

    [Test]
    public void FormulaInventory_AppliesOverflowFormulaImmediately()
    {
        GameState state = new();
        FormulaTileData addOne = new() { operation = FormulaOperator.Add, operand = 1 };

        state.Acquire(addOne);
        state.Acquire(addOne);
        state.Acquire(addOne);

        bool wasStored = state.Acquire(new FormulaTileData { operation = FormulaOperator.Add, operand = 10 });

        Assert.That(wasStored, Is.False);
        Assert.That(state.Score, Is.EqualTo(10));
        Assert.That(state.StoredFormulas, Has.Count.EqualTo(GameState.FormulaCapacity));
    }

    [Test]
    public void StoredFormula_UpdatesScoreAndRemovesOnlyTheSelectedFormula()
    {
        GameState state = new();
        state.Acquire(new FormulaTileData { operation = FormulaOperator.Add, operand = 7 });
        state.Acquire(new FormulaTileData { operation = FormulaOperator.Multiply, operand = 3 });

        bool wasApplied = state.TryApplyStoredFormula(0);

        Assert.That(wasApplied, Is.True);
        Assert.That(state.Score, Is.EqualTo(7));
        Assert.That(state.StoredFormulas, Has.Count.EqualTo(1));
        Assert.That(state.StoredFormulas[0].operation, Is.EqualTo(FormulaOperator.Multiply));
    }

    [Test]
    public void MoveTimer_ReportsExpirationWithoutChangingGameState()
    {
        MoveTimer timer = new();
        timer.Start(1f);

        Assert.That(timer.Tick(.5f), Is.False);
        Assert.That(timer.Remaining, Is.EqualTo(.5f));
        Assert.That(timer.Tick(.5f), Is.True);
        Assert.That(timer.Remaining, Is.EqualTo(0f));
    }

    [Test]
    public void GetCellsOnAnyPath_ExcludesDeadEndBranches()
    {
        Vector3Int start = new(0, 0, 0);
        Vector3Int goal = new(3, 0, 0);
        Vector3Int deadEnd = new(-1, 1, 0);
        HashSet<Vector3Int> cells = new()
        {
            start,
            new Vector3Int(1, 0, 0),
            new Vector3Int(2, 0, 0),
            goal,
            deadEnd
        };

        HashSet<Vector3Int> usableCells = HexPathfinder.GetCellsOnAnyPath(start, goal, cells, new HashSet<Vector3Int>());

        Assert.That(usableCells, Does.Contain(start));
        Assert.That(usableCells, Does.Contain(goal));
        Assert.That(usableCells, Has.None.EqualTo(deadEnd));
    }

    [Test]
    public void StageValidation_RejectsBoardsWithoutARouteToTheGoal()
    {
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        try
        {
            SerializedObject serializedStage = new(stage);
            serializedStage.FindProperty("startCell").vector3IntValue = new Vector3Int(0, 0, 0);
            serializedStage.FindProperty("goalCell").vector3IntValue = new Vector3Int(2, 0, 0);
            serializedStage.ApplyModifiedPropertiesWithoutUndo();

            bool isValid = stage.Validate(out string error);

            Assert.That(isValid, Is.False);
            Assert.That(error, Does.Contain("no path"));
        }
        finally
        {
            Object.DestroyImmediate(stage);
        }
    }
}
