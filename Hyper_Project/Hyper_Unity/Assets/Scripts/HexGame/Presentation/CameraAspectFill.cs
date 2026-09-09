using UnityEngine;

/// <summary>Keeps a background sprite centered on and covering an orthographic camera.</summary>
[ExecuteAlways]
public sealed class CameraAspectFill : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private SpriteRenderer targetRenderer;

    private Vector2Int lastScreenSize;

    private void OnEnable() => Fit();

    private void LateUpdate()
    {
        Vector2Int screenSize = new(Screen.width, Screen.height);
        if (screenSize == lastScreenSize && !Application.isPlaying)
        {
            return;
        }

        Fit();
    }

    private void Fit()
    {
        if (targetCamera == null || targetRenderer == null || targetRenderer.sprite == null || !targetCamera.orthographic)
        {
            return;
        }

        lastScreenSize = new Vector2Int(Screen.width, Screen.height);
        float cameraHeight = targetCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * targetCamera.aspect;
        Vector2 spriteSize = targetRenderer.sprite.bounds.size;
        float scale = Mathf.Max(cameraWidth / spriteSize.x, cameraHeight / spriteSize.y);
        transform.position = new Vector3(targetCamera.transform.position.x, targetCamera.transform.position.y, 1f);
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
