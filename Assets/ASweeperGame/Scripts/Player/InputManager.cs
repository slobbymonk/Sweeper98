using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum KeyboardOrMouse
{
    Keyboard,
    Controller
}
public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActions _input;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public Action OnLaunched;
    public Action OnLaunchReleased;

    public static InputManager Instance;

    public KeyboardOrMouse keyboardOrMouse { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void OnEnable()
    {
        _input = new InputActions();
        _input.Player.Enable();
        _input.Player.Move.performed += SetMove;
        _input.Player.Move.canceled += SetMove;
        _input.Player.Look.performed += SetLook;
        _input.Player.Look.canceled += SetLook;
        _input.Player.Launch.performed += (_) => OnLaunched?.Invoke();
        _input.Player.Launch.canceled += (_) => OnLaunchReleased?.Invoke();
        InputSystem.onActionChange += HandleDeviceChange;
    }

    public void OnDisable()
    {
        _input.Player.Move.performed -= SetMove;
        _input.Player.Move.canceled -= SetMove;
        _input.Player.Look.performed -= SetLook;
        _input.Player.Look.canceled -= SetLook;
        _input.Player.Launch.performed -= (_) => OnLaunched?.Invoke();
        _input.Player.Launch.canceled -= (_) => OnLaunchReleased?.Invoke();
        _input.Player.Disable();
        InputSystem.onActionChange -= HandleDeviceChange;
    }

    private void SetMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }
    private void SetLook(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
    }
    private void HandleDeviceChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed) return;

        var action = obj as InputAction;
        var device = action?.activeControl?.device;

        if (device is Gamepad)
            keyboardOrMouse = KeyboardOrMouse.Controller;
        else if (device is Mouse || device is Keyboard)
            keyboardOrMouse = KeyboardOrMouse.Keyboard;
    }
}
