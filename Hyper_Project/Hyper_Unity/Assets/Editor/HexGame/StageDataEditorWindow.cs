using UnityEditor;
using UnityEngine;

public sealed class StageDataEditorWindow : EditorWindow
{
    private const float MinimumBoardHeight = 100f;
    private const float MinimumDetailsHeight = 90f;
    private const float WindowPadding = 4f;
    private const float ResizeHandleHeight = 8f;
    private const float SectionSpacing = 4f;

    private readonly HexStageEditorBoard board = new();

    private StageData stage;
    private SerializedObject serializedStage;
    private StageDataSerializedEditor stageEditor;
    private Vector3Int selectedCell;
    private float boardHeight = float.MaxValue;
    private bool isResizingBoard;
    private float resizeStartMouseY;
    private float resizeStartBoardHeight;
    private Vector2 detailsScrollPosition;

    [MenuItem("Hex Game/Stage Editor")]
    private static void Open()
    {
        StageDataEditorWindow window = GetWindow<StageDataEditorWindow>("Hex Stage Editor");
        window.minSize = new Vector2(380f, 360f);
    }

    private void OnGUI()
    {
        Rect stageRect = new(WindowPadding, WindowPadding, position.width - WindowPadding * 2f, EditorGUIUtility.singleLineHeight);
        DrawStageField(stageRect);
        if (stage == null)
        {
            EditorGUI.HelpBox(new Rect(WindowPadding, stageRect.yMax + SectionSpacing, stageRect.width, EditorGUIUtility.singleLineHeight * 2f),
                "Select a StageData asset to edit.", MessageType.Info);
            return;
        }

        serializedStage.Update();
        Rect paneRect = new(WindowPadding, stageRect.yMax + SectionSpacing, stageRect.width,
            position.height - stageRect.yMax - SectionSpacing - WindowPadding);
        boardHeight = Mathf.Clamp(boardHeight, MinimumBoardHeight, GetMaximumBoardHeight(paneRect.height));

        Rect boardRect = new(paneRect.x, paneRect.y, paneRect.width, boardHeight);
        Rect handleRect = new(paneRect.x, boardRect.yMax + SectionSpacing, paneRect.width, ResizeHandleHeight);
        Rect detailsRect = new(paneRect.x, handleRect.yMax + SectionSpacing, paneRect.width, paneRect.yMax - handleRect.yMax - SectionSpacing);

        DrawBoard(boardRect);
        DrawBoardResizeHandle(handleRect, paneRect.height);
        DrawDetails(detailsRect);

        if (serializedStage.ApplyModifiedProperties())
        {
            EditorUtility.SetDirty(stage);
        }
    }

    private void DrawStageField(Rect fieldRect)
    {
        EditorGUI.BeginChangeCheck();
        stage = (StageData)EditorGUI.ObjectField(fieldRect, "Stage", stage, typeof(StageData), false);
        if (!EditorGUI.EndChangeCheck())
        {
            return;
        }

        serializedStage = stage == null ? null : new SerializedObject(stage);
        stageEditor = stage == null ? null : new StageDataSerializedEditor(serializedStage);
    }

    private void DrawBoard(Rect boardRect)
    {
        if (!board.Draw(boardRect, stageEditor.GetCells(), selectedCell, stageEditor.GetCellColor, stageEditor.GetCellLabel, out Vector3Int clickedCell))
        {
            return;
        }

        selectedCell = clickedCell;
        stageEditor.SelectOrAddFormula(selectedCell);
        Repaint();
    }

    private void DrawDetails(Rect detailsRect)
    {
        GUI.Box(detailsRect, GUIContent.none, EditorStyles.helpBox);
        GUILayout.BeginArea(detailsRect);
        detailsScrollPosition = EditorGUILayout.BeginScrollView(detailsScrollPosition, false, false,
            GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        stageEditor.DrawSelectedCellEditor(selectedCell);
        stageEditor.DrawValidation(stage);
        EditorGUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private void DrawBoardResizeHandle(Rect handleRect, float paneHeight)
    {
        EditorGUI.DrawRect(handleRect, new Color(.3f, .3f, .3f));
        EditorGUIUtility.AddCursorRect(handleRect, MouseCursor.ResizeVertical);

        Event currentEvent = Event.current;
        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && handleRect.Contains(currentEvent.mousePosition))
        {
            isResizingBoard = true;
            resizeStartMouseY = currentEvent.mousePosition.y;
            resizeStartBoardHeight = boardHeight;
            currentEvent.Use();
            return;
        }

        if (isResizingBoard && currentEvent.type == EventType.MouseDrag)
        {
            boardHeight = Mathf.Clamp(resizeStartBoardHeight + currentEvent.mousePosition.y - resizeStartMouseY,
                MinimumBoardHeight, GetMaximumBoardHeight(paneHeight));
            currentEvent.Use();
            Repaint();
            return;
        }

        if (isResizingBoard && currentEvent.type == EventType.MouseUp)
        {
            isResizingBoard = false;
            currentEvent.Use();
        }
    }

    private static float GetMaximumBoardHeight(float paneHeight)
    {
        return Mathf.Max(MinimumBoardHeight, paneHeight - MinimumDetailsHeight - ResizeHandleHeight - SectionSpacing * 2f);
    }
}
