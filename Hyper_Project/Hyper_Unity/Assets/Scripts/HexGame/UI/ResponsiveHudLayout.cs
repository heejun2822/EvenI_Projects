using UnityEngine;

/// <summary>Reflows the HUD cards when the canvas changes between landscape and portrait proportions.</summary>
[ExecuteAlways]
public sealed class ResponsiveHudLayout : MonoBehaviour
{
    [SerializeField] private RectTransform goalCard;
    [SerializeField] private RectTransform scoreCard;
    [SerializeField] private RectTransform healthCard;
    [SerializeField] private RectTransform timerCard;
    [SerializeField] private RectTransform formulaDock;

    private Vector2 lastSize;

    private void OnEnable() => ApplyLayout();
    private void OnRectTransformDimensionsChange() => ApplyLayout();

    private void ApplyLayout()
    {
        RectTransform root = (RectTransform)transform;
        Vector2 size = root.rect.size;
        if (size == lastSize || size.y <= 0f)
        {
            return;
        }

        lastSize = size;
        if (size.x / size.y < 1.1f)
        {
            SetCard(goalCard, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(18f, -18f), new Vector2(210f, 78f));
            SetCard(scoreCard, new Vector2(.5f, 1f), new Vector2(.5f, 1f), new Vector2(0f, -18f), new Vector2(210f, 78f));
            SetCard(healthCard, Vector2.one, Vector2.one, new Vector2(-18f, -18f), new Vector2(210f, 78f));
            SetCard(timerCard, new Vector2(.5f, 1f), new Vector2(.5f, 1f), new Vector2(0f, -108f), new Vector2(150f, 72f));
            SetCard(formulaDock, new Vector2(.5f, 0f), new Vector2(.5f, 0f), new Vector2(0f, 20f), new Vector2(620f, 112f));
            return;
        }

        SetCard(goalCard, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(28f, -28f), new Vector2(230f, 88f));
        SetCard(scoreCard, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(276f, -28f), new Vector2(230f, 88f));
        SetCard(healthCard, Vector2.one, Vector2.one, new Vector2(-28f, -28f), new Vector2(230f, 88f));
        SetCard(timerCard, new Vector2(.5f, 1f), new Vector2(.5f, 1f), new Vector2(0f, -28f), new Vector2(170f, 88f));
        SetCard(formulaDock, new Vector2(.5f, 0f), new Vector2(.5f, 0f), new Vector2(0f, 24f), new Vector2(660f, 126f));
    }

    private static void SetCard(RectTransform card, Vector2 anchor, Vector2 pivot, Vector2 position, Vector2 size)
    {
        if (card == null)
        {
            return;
        }

        card.anchorMin = card.anchorMax = anchor;
        card.pivot = pivot;
        card.anchoredPosition = position;
        card.sizeDelta = size;
    }
}
