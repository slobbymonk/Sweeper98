using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActions _input;

    public Vector2 MoveInput { get; private set; }
    public Action OnLaunched;
    public Action OnLaunchReleased;

    public static InputManager Instance;

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
        _input.Player.Launch.performed += (_) => OnLaunched?.Invoke();
        _input.Player.Launch.canceled += (_) => OnLaunchReleased?.Invoke();
    }

    public void OnDisable()
    {
        _input.Player.Move.performed -= SetMove;
        _input.Player.Move.canceled -= SetMove;
        _input.Player.Launch.performed -= (_) => OnLaunched?.Invoke();
        _input.Player.Launch.canceled -= (_) => OnLaunchReleased?.Invoke();
        _input.Player.Disable();
    }

    private void SetMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }
}
