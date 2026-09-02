using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MoveGlowPulse : MonoBehaviour
{
    [SerializeField] private float minimumAlpha = .12f;
    [SerializeField] private float maximumAlpha = .48f;
    [SerializeField] private float pulseSpeed = 6f;

    private SpriteRenderer spriteRenderer;
    private Color glowColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        glowColor = spriteRenderer.color;
    }

    private void Update()
    {
        float wave = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * .5f;
        glowColor.a = Mathf.Lerp(minimumAlpha, maximumAlpha, wave);
        spriteRenderer.color = glowColor;
    }
}
