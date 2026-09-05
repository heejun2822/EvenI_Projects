using System.Collections.Generic;
using UnityEngine;

/// <summary>Keeps a Canvas's direct UI children inside the device safe area.</summary>
public sealed class SafeAreaFitter : MonoBehaviour
{
    private readonly List<RectTransform> children = new();
    private readonly List<Vector2> anchorMins = new();
    private readonly List<Vector2> anchorMaxs = new();
    private Rect lastSafeArea;
    private Vector2Int lastScreenSize;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child is not RectTransform rectTransform)
            {
                continue;
            }

            children.Add(rectTransform);
            anchorMins.Add(rectTransform.anchorMin);
            anchorMaxs.Add(rectTransform.anchorMax);
        }
    }

    private void OnEnable()
    {
        ApplySafeArea(force: true);
    }

    private void Update()
    {
        ApplySafeArea(force: false);
    }

    private void ApplySafeArea(bool force)
    {
        Rect safeArea = Screen.safeArea;
        Vector2Int screenSize = new(Screen.width, Screen.height);
        if (!force && safeArea == lastSafeArea && screenSize == lastScreenSize)
        {
            return;
        }

        lastSafeArea = safeArea;
        lastScreenSize = screenSize;
        Vector2 safeMin = new(safeArea.xMin / screenSize.x, safeArea.yMin / screenSize.y);
        Vector2 safeSize = new(safeArea.width / screenSize.x, safeArea.height / screenSize.y);
        for (int index = 0; index < children.Count; index++)
        {
            children[index].anchorMin = safeMin + Vector2.Scale(safeSize, anchorMins[index]);
            children[index].anchorMax = safeMin + Vector2.Scale(safeSize, anchorMaxs[index]);
        }
    }
}
