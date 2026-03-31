using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 4500;
    [SerializeField] private float _maxSpeed = 20;
    [SerializeField] private float _counterMovement = 0.175f;
    private float _threshold = 0.01f;
    private Rigidbody2D _rb;

    [Header("Input")]
    public Vector2 MoveInput;
    [SerializeField] private InputActions _input;

    [Header("Assignables")]
    public Transform _orientation;

    void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void FixedUpdate()
    {
        Movement();
    }

    public void OnEnable()
    {
        _input = new InputActions();
        _input.Player.Enable();
        _input.Player.Move.performed += SetMove;
        _input.Player.Move.canceled += SetMove;
    }

    public void OnDisable()
    {
        _input.Player.Move.performed -= SetMove;
        _input.Player.Move.canceled -= SetMove;
        _input.Player.Disable();
    }

    private void SetMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void Movement()
    {
        float x = MoveInput.x;
        float y = MoveInput.y;

        Vector2 mag = _rb.linearVelocity;
        float xMag = mag.x, yMag = mag.y;

        CounterMovement(x, y, mag);

        // Clamp input at max speed
        if (x > 0 && xMag > _maxSpeed) x = 0;
        if (x < 0 && xMag < -_maxSpeed) x = 0;
        if (y > 0 && yMag > _maxSpeed) y = 0;
        if (y < 0 && yMag < -_maxSpeed) y = 0;

        float multiplier = 1f, multiplierV = 1f;

        _rb.AddForce(_orientation.transform.up * y * _moveSpeed * Time.deltaTime * multiplier * multiplierV);
        _rb.AddForce(_orientation.transform.right * x * _moveSpeed * Time.deltaTime * multiplier);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (Math.Abs(mag.x) > _threshold && Math.Abs(x) < 0.05f
            || (mag.x < -_threshold && x > 0)
            || (mag.x > _threshold && x < 0))
        {
            _rb.AddForce(_moveSpeed * _orientation.transform.right * Time.deltaTime * -mag.x * _counterMovement);
        }

        if (Math.Abs(mag.y) > _threshold && Math.Abs(y) < 0.05f
            || (mag.y < -_threshold && y > 0)
            || (mag.y > _threshold && y < 0))
        {
            _rb.AddForce(_moveSpeed * _orientation.transform.up * Time.deltaTime * -mag.y * _counterMovement);
        }

        if (Mathf.Sqrt(Mathf.Pow(_rb.linearVelocity.x, 2) + Mathf.Pow(_rb.linearVelocity.y, 2)) > _maxSpeed)
        {
            Vector2 n = _rb.linearVelocity.normalized * _maxSpeed;
            _rb.linearVelocity = new Vector2(n.x, n.y);
        }
    }
}