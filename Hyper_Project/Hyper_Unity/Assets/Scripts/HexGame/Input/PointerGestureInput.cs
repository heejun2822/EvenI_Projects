using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using EnhancedTouchSupport = UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport;

public class PointerGestureInput : BaseEntity
{
    [SerializeField] private float dragThreshold = 15f;
    [SerializeField] private float pinchZoomScale = .02f;

    public event Action<Vector2> Tapped;
    public event Action<Vector2> Dragged;
    public event Action<Vector2> Zoomed;

    private PlayerInputActions inputActions;
    private InputAction clickAction;
    private InputAction pointerPositionAction;
    private InputAction zoomAction;
    private bool gestureStartedOverUi;
    private bool isPressed;
    private bool isDragging;
    private bool isPointerOverUi;
    private bool isPinching;
    private bool pinchStartedOverUi;
    private float previousPinchDistance;
    private Vector2 pressPosition;
    private Vector2 previousPosition;

    protected override void Awake()
    {
        base.Awake();
        EnhancedTouchSupport.Enable();
        inputActions = new PlayerInputActions();
        clickAction = inputActions.Player.Click;
        pointerPositionAction = inputActions.Player.PointerPosition;
        zoomAction = inputActions.Player.Zoom;
    }

    public void Initialize()
    {
        CompleteInitialization();
    }

    private void OnEnable()
    {
        if (inputActions == null)
        {
            return;
        }

        inputActions.Enable();
        clickAction.started += OnClickStarted;
        clickAction.canceled += OnClickCanceled;
        pointerPositionAction.performed += OnPointerPosition;
        zoomAction.performed += OnZoom;
    }

    private void Update()
    {
        isPointerOverUi = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        UpdatePinchZoom();
    }

    private void OnDisable()
    {
        if (inputActions == null)
        {
            return;
        }

        clickAction.started -= OnClickStarted;
        clickAction.canceled -= OnClickCanceled;
        pointerPositionAction.performed -= OnPointerPosition;
        zoomAction.performed -= OnZoom;
        inputActions.Disable();
    }

    private void OnDestroy()
    {
        inputActions?.Dispose();
        EnhancedTouchSupport.Disable();
    }

    private void OnClickStarted(InputAction.CallbackContext context)
    {
        gestureStartedOverUi = isPointerOverUi;
        isPressed = true;
        isDragging = false;
        pressPosition = pointerPositionAction.ReadValue<Vector2>();
        previousPosition = pressPosition;
    }

    private void OnClickCanceled(InputAction.CallbackContext context)
    {
        if (isPressed && !isDragging && !gestureStartedOverUi && !isPointerOverUi)
        {
            Tapped?.Invoke(pointerPositionAction.ReadValue<Vector2>());
        }
        isPressed = false;
    }

    private void OnPointerPosition(InputAction.CallbackContext context)
    {
        if (!isPressed || gestureStartedOverUi || isPinching)
        {
            return;
        }

        Vector2 position = context.ReadValue<Vector2>();
        if (!isDragging && Vector2.Distance(pressPosition, position) >= dragThreshold)
        {
            isDragging = true;
        }
        if (isDragging)
        {
            Dragged?.Invoke(position - previousPosition);
        }
        previousPosition = position;
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        if (!isPointerOverUi)
        {
            Zoomed?.Invoke(context.ReadValue<Vector2>());
        }
    }

    private void UpdatePinchZoom()
    {
        if (EnhancedTouch.activeTouches.Count < 2)
        {
            isPinching = false;
            return;
        }

        float pinchDistance = Vector2.Distance(
            EnhancedTouch.activeTouches[0].screenPosition,
            EnhancedTouch.activeTouches[1].screenPosition);
        if (!isPinching)
        {
            pinchStartedOverUi = isPointerOverUi;
            previousPinchDistance = pinchDistance;
            isPinching = true;
            isDragging = true;
            return;
        }

        if (!pinchStartedOverUi)
        {
            Zoomed?.Invoke(new Vector2(0f, (pinchDistance - previousPinchDistance) * pinchZoomScale));
        }

        previousPinchDistance = pinchDistance;
    }
}
