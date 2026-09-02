using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

internal sealed class StageDataSerializedEditor
{
    private readonly SerializedObject serializedStage;

    public StageDataSerializedEditor(SerializedObject serializedStage)
    {
        this.serializedStage = serializedStage;
    }

    public Vector3Int StartCell => Find("startCell").vector3IntValue;
    public Vector3Int GoalCell => Find("goalCell").vector3IntValue;

    public List<Vector3Int> GetCells()
    {
        List<Vector3Int> cells = new();
        AddCells(cells, FormulaTiles);
        AddCells(cells, HealthTiles);
        cells.Add(GoalCell);
        cells.Add(StartCell);
        return cells;
    }

    public Color GetCellColor(Vector3Int cell)
    {
        if (cell == GoalCell)
        {
            return new Color(.8f, .25f, .25f);
        }

        if (cell == StartCell)
        {
            return new Color(.2f, .55f, 1f);
        }

        return IsHealthCell(cell) ? new Color(.3f, .85f, .5f) : new Color(.85f, .65f, .15f);
    }

    public string GetCellLabel(Vector3Int cell)
    {
        string coordinate = $"({cell.x}, {cell.y})";
        if (cell == StartCell)
        {
            return coordinate + "\nSTART";
        }

        if (cell == GoalCell)
        {
            return coordinate + $"\nGOAL: {Find("goalScore").intValue}";
        }

        int formulaIndex = FindTileIndex(FormulaTiles, cell);
        if (formulaIndex >= 0)
        {
            FormulaTileData tile = (FormulaTileData)FormulaTiles.GetArrayElementAtIndex(formulaIndex).boxedValue;
            return coordinate + "\n" + tile;
        }

        int healthIndex = FindTileIndex(HealthTiles, cell);
        if (healthIndex >= 0)
        {
            int healthAmount = HealthTiles.GetArrayElementAtIndex(healthIndex).FindPropertyRelative("healthAmount").intValue;
            return coordinate + $"\nHEAL +{healthAmount}";
        }

        return coordinate;
    }

    public void SelectOrAddFormula(Vector3Int cell)
    {
        if (!GetCells().Contains(cell))
        {
            AddFormula(cell);
        }
    }

    public void DrawSelectedCellEditor(Vector3Int selectedCell)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Selected Cell", selectedCell.ToString());
        int formulaIndex = FindTileIndex(FormulaTiles, selectedCell);
        int healthIndex = FindTileIndex(HealthTiles, selectedCell);
        if (selectedCell == GoalCell)
        {
            EditorGUILayout.PropertyField(Find("goalScore"), new GUIContent("Goal Score"));
            return;
        }

        if (formulaIndex >= 0)
        {
            DrawFormulaCellEditor(formulaIndex, selectedCell);
            return;
        }

        if (healthIndex >= 0)
        {
            DrawHealthCellEditor(healthIndex, selectedCell);
            return;
        }

        if (selectedCell == StartCell)
        {
            EditorGUILayout.HelpBox("The start cell cannot contain a formula or health tile.", MessageType.None);
            return;
        }

        EditorGUILayout.HelpBox("This cell is empty. Click the board to add a formula tile, or add a health tile below.", MessageType.None);
        if (GUILayout.Button("Add Health Tile"))
        {
            AddHealth(selectedCell);
        }
    }

    public void DrawValidation(StageData stage)
    {
        EditorGUILayout.Space();
        if (stage.Validate(out string error))
        {
            EditorGUILayout.HelpBox("Stage data is valid.", MessageType.Info);
            return;
        }

        EditorGUILayout.HelpBox(error, MessageType.Error);
    }

    private SerializedProperty FormulaTiles => Find("formulaTiles");
    private SerializedProperty HealthTiles => Find("healthTiles");

    private SerializedProperty Find(string propertyName)
    {
        return serializedStage.FindProperty(propertyName);
    }

    private void DrawFormulaCellEditor(int formulaIndex, Vector3Int selectedCell)
    {
        SerializedProperty tile = FormulaTiles.GetArrayElementAtIndex(formulaIndex);
        EditorGUILayout.PropertyField(tile.FindPropertyRelative("operation"), new GUIContent("Operation"));
        EditorGUILayout.PropertyField(tile.FindPropertyRelative("operand"), new GUIContent("Operand"));
        DrawOccupiedCellActions(FormulaTiles, formulaIndex, selectedCell);

        if (GUILayout.Button("Convert to Health Tile"))
        {
            FormulaTiles.DeleteArrayElementAtIndex(formulaIndex);
            AddHealth(selectedCell);
        }
    }

    private void DrawHealthCellEditor(int healthIndex, Vector3Int selectedCell)
    {
        SerializedProperty tile = HealthTiles.GetArrayElementAtIndex(healthIndex);
        EditorGUILayout.PropertyField(tile.FindPropertyRelative("healthAmount"), new GUIContent("Health Restored"));
        DrawOccupiedCellActions(HealthTiles, healthIndex, selectedCell);

        if (GUILayout.Button("Convert to Formula Tile"))
        {
            HealthTiles.DeleteArrayElementAtIndex(healthIndex);
            AddFormula(selectedCell);
        }
    }

    private void DrawOccupiedCellActions(SerializedProperty tiles, int tileIndex, Vector3Int selectedCell)
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Set Start"))
            {
                SetSpecialCell("startCell", tiles, tileIndex, selectedCell);
            }

            if (GUILayout.Button("Set Goal"))
            {
                SetSpecialCell("goalCell", tiles, tileIndex, selectedCell);
            }

            if (GUILayout.Button("Delete Cell"))
            {
                tiles.DeleteArrayElementAtIndex(tileIndex);
            }
        }
    }

    private void SetSpecialCell(string propertyName, SerializedProperty tiles, int tileIndex, Vector3Int selectedCell)
    {
        Find(propertyName).vector3IntValue = selectedCell;
        tiles.DeleteArrayElementAtIndex(tileIndex);
    }

    private void AddFormula(Vector3Int cell)
    {
        FormulaTiles.arraySize++;
        SerializedProperty tile = FormulaTiles.GetArrayElementAtIndex(FormulaTiles.arraySize - 1);
        tile.FindPropertyRelative("cell").vector3IntValue = cell;
        tile.FindPropertyRelative("operation").enumValueIndex = (int)FormulaOperator.Add;
        tile.FindPropertyRelative("operand").intValue = 1;
    }

    private void AddHealth(Vector3Int cell)
    {
        HealthTiles.arraySize++;
        SerializedProperty tile = HealthTiles.GetArrayElementAtIndex(HealthTiles.arraySize - 1);
        tile.FindPropertyRelative("cell").vector3IntValue = cell;
        tile.FindPropertyRelative("healthAmount").intValue = 10;
    }

    private bool IsHealthCell(Vector3Int cell)
    {
        return FindTileIndex(HealthTiles, cell) >= 0;
    }

    private static void AddCells(List<Vector3Int> cells, SerializedProperty tiles)
    {
        for (int index = 0; index < tiles.arraySize; index++)
        {
            cells.Add(tiles.GetArrayElementAtIndex(index).FindPropertyRelative("cell").vector3IntValue);
        }
    }

    private static int FindTileIndex(SerializedProperty tiles, Vector3Int cell)
    {
        for (int index = 0; index < tiles.arraySize; index++)
        {
            if (tiles.GetArrayElementAtIndex(index).FindPropertyRelative("cell").vector3IntValue == cell)
            {
                return index;
            }
        }

        return -1;
    }
}
