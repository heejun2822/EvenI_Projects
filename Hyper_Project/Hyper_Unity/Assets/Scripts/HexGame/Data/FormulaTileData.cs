using System;
using UnityEngine;

public enum FormulaOperator
{
    Add,
    Subtract,
    Multiply,
    Divide
}

[Serializable]
public struct FormulaTileData
{
    public Vector3Int cell;
    public FormulaOperator operation;
    public int operand;

    public readonly int Apply(int score)
    {
        return operation switch
        {
            FormulaOperator.Add => score + operand,
            FormulaOperator.Subtract => score - operand,
            FormulaOperator.Multiply => score * operand,
            FormulaOperator.Divide => score / operand,
            _ => score
        };
    }

    public override readonly string ToString()
    {
        string symbol = operation switch
        {
            FormulaOperator.Add => "+",
            FormulaOperator.Subtract => "-",
            FormulaOperator.Multiply => "×",
            FormulaOperator.Divide => "÷",
            _ => "?"
        };
        return symbol + operand;
    }
}
