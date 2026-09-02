using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PointerGestureInput : MonoBehaviour
{
    [SerializeField] private float dragThreshold = 15f;

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
    private Vector2 pressPosition;
    private Vector2 previousPosition;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        clickAction = inputActions.Player.Click;
        pointerPositionAction = inputActions.Player.PointerPosition;
        zoomAction = inputActions.Player.Zoom;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        clickAction.started += OnClickStarted;
        clickAction.canceled += OnClickCanceled;
        pointerPositionAction.performed += OnPointerPosition;
        zoomAction.performed += OnZoom;
    }

    private void Update()
    {
        isPointerOverUi = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private void OnDisable()
    {
        clickAction.started -= OnClickStarted;
        clickAction.canceled -= OnClickCanceled;
        pointerPositionAction.performed -= OnPointerPosition;
        zoomAction.performed -= OnZoom;
        inputActions.Disable();
    }

    private void OnDestroy()
    {
        inputActions.Dispose();
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
        if (!isPressed || gestureStartedOverUi)
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
}
