using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public InputActions inputActions;
    public PlayerInput playerInput;

    //#if UNITY_STANDALONE_WIN

    private void OnEnable()
    {
        InputActions inputActions = new InputActions();
        inputActions.Game.Enable();
        inputActions.Game.ChangeResolution.performed += ChangeResolution_canceled;
    }

    private void OnDisable()
    {
        inputActions.Game.ChangeResolution.performed -= ChangeResolution_canceled;
    }

    private void ChangeResolution_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GameStateManager.instance.ChangeRes();

    }

    private void Awake()
    {

        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();
    }

    //#endif
}
