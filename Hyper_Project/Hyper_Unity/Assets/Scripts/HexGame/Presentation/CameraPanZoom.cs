using UnityEngine;

public class CameraPanZoom : MonoBehaviour
{
    [SerializeField] private float panSpeed = .001f;
    [SerializeField] private float zoomSpeed = .2f;
    [SerializeField] private float minimumZoom = 3f;
    [SerializeField] private float maximumZoom = 7f;
    [SerializeField] private float stagePadding = .25f;

    private Camera targetCamera;
    private Bounds stageBounds;
    private Vector2 panBoundsOffset;
    private bool hasStageBounds;

    private void Awake()
    {
        targetCamera = GetComponent<Camera>();
    }

    public void ResetForStage(Bounds bounds, Vector3 startPosition, Vector2 panOffset)
    {
        stageBounds = bounds;
        panBoundsOffset = panOffset;
        hasStageBounds = true;

        float fittedZoom = Mathf.Max(
            (stageBounds.size.y * .5f) + stagePadding,
            (stageBounds.size.x / (2f * targetCamera.aspect)) + stagePadding);
        targetCamera.orthographicSize = Mathf.Clamp(fittedZoom, minimumZoom, maximumZoom);

        Vector3 position = stageBounds.center;
        if (fittedZoom > maximumZoom)
        {
            position = startPosition;
        }
        position.z = targetCamera.transform.position.z;
        targetCamera.transform.position = ClampPosition(position);
    }

    private void OnEnable()
    {
        EventBus<PointerDraggedEvent>.Subscribe(Pan);
        EventBus<PointerZoomedEvent>.Subscribe(Zoom);
    }

    private void OnDisable()
    {
        EventBus<PointerDraggedEvent>.Unsubscribe(Pan);
        EventBus<PointerZoomedEvent>.Unsubscribe(Zoom);
    }

    private void Pan(PointerDraggedEvent payload)
    {
        Vector3 position = targetCamera.transform.position - (Vector3)(payload.Delta * panSpeed * targetCamera.orthographicSize);
        targetCamera.transform.position = ClampPosition(position);
    }

    private void Zoom(PointerZoomedEvent payload)
    {
        targetCamera.orthographicSize = Mathf.Clamp(targetCamera.orthographicSize - payload.Delta.y * zoomSpeed, minimumZoom, maximumZoom);
        targetCamera.transform.position = ClampPosition(targetCamera.transform.position);
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        if (!hasStageBounds)
        {
            return position;
        }

        float halfHeight = targetCamera.orthographicSize;
        float halfWidth = halfHeight * targetCamera.aspect;
        position.x = ClampAxis(position.x, stageBounds.min.x - panBoundsOffset.x, stageBounds.max.x + panBoundsOffset.x, halfWidth);
        position.y = ClampAxis(position.y, stageBounds.min.y - panBoundsOffset.y, stageBounds.max.y + panBoundsOffset.y, halfHeight);
        return position;
    }

    private static float ClampAxis(float value, float minimum, float maximum, float halfViewSize)
    {
        float minimumCenter = minimum + halfViewSize;
        float maximumCenter = maximum - halfViewSize;
        return minimumCenter > maximumCenter ? (minimum + maximum) * .5f : Mathf.Clamp(value, minimumCenter, maximumCenter);
    }

}
