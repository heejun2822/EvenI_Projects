using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

internal sealed class HexStageEditorBoard
{
    private static GUIStyle cellLabelStyle;

    private Vector2 pan;
    private float zoom = 1f;
    private bool isPanning;
    private Vector2 previousMousePosition;

    public bool Draw(Rect boardRect, IReadOnlyList<Vector3Int> cells, Vector3Int selectedCell,
        Func<Vector3Int, Color> getCellColor, Func<Vector3Int, string> getCellLabel, out Vector3Int clickedCell)
    {
        EditorGUI.DrawRect(boardRect, new Color(.12f, .12f, .12f));
        HandleNavigation(boardRect, Event.current);

        GUI.BeginClip(boardRect);
        Rect clippedBoardRect = new(0f, 0f, boardRect.width, boardRect.height);
        foreach (Vector3Int cell in cells)
        {
            Rect hexRect = GetHexRect(clippedBoardRect, cell);
            DrawHex(hexRect, cell == selectedCell ? Color.green : getCellColor(cell));
            GUI.Label(hexRect, getCellLabel(cell), CellLabelStyle);
        }
        GUI.EndClip();

        clickedCell = default;
        Event currentEvent = Event.current;
        if (currentEvent.type != EventType.MouseDown || currentEvent.button != 0 || !boardRect.Contains(currentEvent.mousePosition))
        {
            return false;
        }

        clickedCell = GetNearestCell(boardRect, currentEvent.mousePosition);
        currentEvent.Use();
        return true;
    }

    private void HandleNavigation(Rect boardRect, Event currentEvent)
    {
        if (!boardRect.Contains(currentEvent.mousePosition))
        {
            return;
        }

        if (currentEvent.type == EventType.ScrollWheel)
        {
            zoom = Mathf.Clamp(zoom - currentEvent.delta.y * .05f, .5f, 2.5f);
            currentEvent.Use();
            return;
        }

        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 2)
        {
            isPanning = true;
            previousMousePosition = currentEvent.mousePosition;
            currentEvent.Use();
            return;
        }

        if (isPanning && currentEvent.type == EventType.MouseDrag && currentEvent.button == 2)
        {
            pan += currentEvent.mousePosition - previousMousePosition;
            previousMousePosition = currentEvent.mousePosition;
            currentEvent.Use();
            return;
        }

        if (isPanning && currentEvent.type == EventType.MouseUp && currentEvent.button == 2)
        {
            isPanning = false;
            currentEvent.Use();
        }
    }

    private Rect GetHexRect(Rect board, Vector3Int cell)
    {
        float size = 42f * zoom;
        float x = board.center.x + pan.x + (cell.x + ((cell.y & 1) == 0 ? 0f : .5f)) * size;
        float y = board.center.y + pan.y - cell.y * size * .76f;
        return new Rect(x - size * .45f, y - size * .4f, size * .9f, size * .8f);
    }

    private Vector3Int GetNearestCell(Rect board, Vector2 point)
    {
        float size = 42f * zoom;
        int y = Mathf.RoundToInt((board.center.y + pan.y - point.y) / (size * .76f));
        int x = Mathf.RoundToInt((point.x - board.center.x - pan.x) / size - ((y & 1) == 0 ? 0f : .5f));
        return new Vector3Int(x, y, 0);
    }

    private static GUIStyle CellLabelStyle
    {
        get
        {
            if (cellLabelStyle != null)
            {
                return cellLabelStyle;
            }

            cellLabelStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };
            SetTextColor(cellLabelStyle, Color.black);
            return cellLabelStyle;
        }
    }

    private static void SetTextColor(GUIStyle style, Color color)
    {
        style.normal.textColor = color;
        style.hover.textColor = color;
        style.active.textColor = color;
        style.focused.textColor = color;
        style.onNormal.textColor = color;
        style.onHover.textColor = color;
        style.onActive.textColor = color;
        style.onFocused.textColor = color;
    }

    private static void DrawHex(Rect rect, Color color)
    {
        Vector3[] points =
        {
            new(rect.center.x, rect.yMin), new(rect.xMax, rect.yMin + rect.height * .25f), new(rect.xMax, rect.yMax - rect.height * .25f),
            new(rect.center.x, rect.yMax), new(rect.xMin, rect.yMax - rect.height * .25f), new(rect.xMin, rect.yMin + rect.height * .25f)
        };
        Color previousColor = Handles.color;
        Handles.color = color;
        Handles.DrawAAConvexPolygon(points);
        Handles.color = previousColor;
        Handles.DrawPolyLine(points[0], points[1], points[2], points[3], points[4], points[5], points[0]);
    }
}
