using UnityEngine;

public class CameraPanZoom : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private float panSpeed = .001f;
    [SerializeField] private float zoomSpeed = .2f;
    [SerializeField] private float minimumZoom = 3f;
    [SerializeField] private float maximumZoom = 7f;
    [SerializeField] private float stagePadding = .25f;

    private PointerGestureInput gestureInput;
    private Bounds stageBounds;
    private Vector2 panBoundsOffset;
    private bool hasStageBounds;

    public void BindGestureInput(PointerGestureInput input)
    {
        UnbindGestureInput();
        gestureInput = input;
        if (isActiveAndEnabled)
        {
            SubscribeGestureInput();
        }
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

    private void Reset()
    {
        targetCamera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        SubscribeGestureInput();
    }

    private void OnDisable()
    {
        UnsubscribeGestureInput();
    }

    private void OnDestroy()
    {
        UnbindGestureInput();
    }

    private void SubscribeGestureInput()
    {
        if (gestureInput == null)
        {
            return;
        }
        gestureInput.Dragged += Pan;
        gestureInput.Zoomed += Zoom;
    }

    private void UnsubscribeGestureInput()
    {
        if (gestureInput == null)
        {
            return;
        }
        gestureInput.Dragged -= Pan;
        gestureInput.Zoomed -= Zoom;
    }

    public void UnbindGestureInput()
    {
        UnsubscribeGestureInput();
        gestureInput = null;
    }

    private void Pan(Vector2 delta)
    {
        Vector3 position = targetCamera.transform.position - (Vector3)(delta * panSpeed * targetCamera.orthographicSize);
        targetCamera.transform.position = ClampPosition(position);
    }

    private void Zoom(Vector2 delta)
    {
        targetCamera.orthographicSize = Mathf.Clamp(targetCamera.orthographicSize - delta.y * zoomSpeed, minimumZoom, maximumZoom);
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
