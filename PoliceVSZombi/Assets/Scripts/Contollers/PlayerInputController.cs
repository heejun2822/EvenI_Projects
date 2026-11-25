using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputController : BaseChracterController
{
    private Camera _camera;
    GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void OnMove(InputValue value)
    {
        if (gameManager.IsPlaying == false)
            return;

        // Debug.Log("OnMove" + value.ToString());
        Vector2 moveInput = value.Get<Vector2>().normalized;
        CallMoveEvent(moveInput);
    }

    public void OnLook(InputValue value)
    {
        if (gameManager.IsPlaying == false)
            return;

        // Debug.Log("OnLook" + value.ToString());
        Vector2 newAim = value.Get<Vector2>();
        Vector2 worldPos= _camera.ScreenToWorldPoint(newAim);
        newAim = (worldPos - (Vector2)transform.position).normalized;

        if(newAim.magnitude >= .9f)
        {
            CallLookEvnet(newAim);
        }
    }

    public void OnFire(InputValue value)
    {
        if (gameManager.IsPlaying == false)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        IsAttacking = value.isPressed;
    }

}
