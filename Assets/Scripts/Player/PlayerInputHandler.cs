using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool IsHoldingJump { get; private set; }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = new Vector2(context.ReadValue<Vector2>().x, 0);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        IsHoldingJump = context.performed;
    }
}