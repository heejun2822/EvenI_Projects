using UnityEngine;

/// <summary>Fits this RectTransform to the device safe area.</summary>
[RequireComponent(typeof(RectTransform))]
public sealed class SafeAreaFitter : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect lastSafeArea;
    private Vector2Int lastScreenSize;

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
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
        if (screenSize.x <= 0 || screenSize.y <= 0)
        {
            return;
        }

        if (!force && safeArea == lastSafeArea && screenSize == lastScreenSize)
        {
            return;
        }

        lastSafeArea = safeArea;
        lastScreenSize = screenSize;
        rectTransform.anchorMin = new Vector2(
            safeArea.xMin / screenSize.x,
            safeArea.yMin / screenSize.y);
        rectTransform.anchorMax = new Vector2(
            safeArea.xMax / screenSize.x,
            safeArea.yMax / screenSize.y);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}
