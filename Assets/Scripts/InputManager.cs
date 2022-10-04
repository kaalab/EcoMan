using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    #region Events
    public delegate void Touch(Vector2 position, double time);
    public event Touch OnStartTouch;
    public event Touch OnEndTouch;
    public event Touch OnChangePosition;
    #endregion

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();    
    }

    private void OnEnable()
    {
        playerControls.Enable();
        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }


    void Start()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
        }


        playerControls.Touch.PrimaryContact.started += (ctx) => StarTouchPrimary(ctx);
        playerControls.Touch.PrimaryContact.canceled += (ctx) => EndTouchPrimary(ctx);
        playerControls.Touch.PrimaryPosition.performed += (ctx) => ChangePositionPrimary(ctx);
    }

    private void ChangePositionPrimary(InputAction.CallbackContext ctx)
    {
        OnChangePosition?.Invoke(PrimaryPosition(), ctx.startTime);
    }

    private void StarTouchPrimary(InputAction.CallbackContext ctx)
    {
        OnStartTouch?.Invoke(PrimaryPosition(), ctx.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        OnEndTouch?.Invoke(PrimaryPosition(), ctx.time);
    }

    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
    }

}
