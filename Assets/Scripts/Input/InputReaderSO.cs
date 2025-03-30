using RC;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader", fileName = "InputReader")]
public class InputReaderSO : ScriptableObject, GameInput.IGameplayActions
{
    private GameInput input;

    public event Action<Vector2> LookEvent;
    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> PowerEvent;

    private void OnEnable()
    {
        if (input == null)
        {
            input = new GameInput();
            input.Gameplay.SetCallbacks(this);
            EnableGameplayInput();
        }
    }
    private void OnDisable()
    {
        DisableAllInput();
    }
    public void EnableGameplayInput()
    {
        input.Gameplay.Enable();
    }
    public void DisableAllInput()
    {
        input.Gameplay.Disable();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            LookEvent?.Invoke(context.ReadValue<Vector2>());
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            LookEvent?.Invoke(Vector2.zero);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
        if (context.phase == InputActionPhase.Canceled) 
        {
            MoveEvent?.Invoke(Vector2.zero);
        }
    }
    public void OnPower(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PowerEvent?.Invoke(context.ReadValue<Vector2>());
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            PowerEvent?.Invoke(Vector2.zero);
        }
    }

    // Đối với joystick hay input kiểu Vector2 gọi trực tiếp:
    public void OnLook(Vector2 lookInput)
    {
        if (lookInput != Vector2.zero)
        {
            LookEvent?.Invoke(lookInput);
        }
        if (lookInput == Vector2.zero)
        {
            LookEvent?.Invoke(Vector2.zero);
        }
    }
    public void OnMove(Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            MoveEvent?.Invoke(moveInput);
        }
        if (moveInput == Vector2.zero)
        {
            MoveEvent?.Invoke(Vector2.zero);
        }
    }
    public void OnPower(Vector2 powerInput)
    {
        if (powerInput != Vector2.zero)
        {
            PowerEvent?.Invoke(powerInput);
        }
        if (powerInput == Vector2.zero)
        {
            PowerEvent?.Invoke(Vector2.zero);
        }
    }
    // Có thể bổ sung thêm các phương thức khác nếu cần thiết.
}
