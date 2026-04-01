using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MineGrabber : MonoBehaviour
{
    private Mine _currentlyHeldMine;
    [SerializeField] private Sprite _bombHoldingSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Launch")]
    [SerializeField] private float _minLaunchForce = 500f;
    [SerializeField] private float _maxLaunchForce = 2000f;
    [SerializeField] private float _maxChargeTime = 2f;
    [SerializeField] private Transform _rotationPosition;
    [SerializeField] private Transform _holdPosition;

    [Header("Shake")]
    [SerializeField] private float _maxShakeAmount = 0.15f;
    [SerializeField] private float _shakeSpeed = 30f;

    private Sprite _defaultSprite;
    private bool _isCoolingDown;
    private bool _isCharging;
    private float _chargeTime;
    private Vector3 _heldMineBaseLocalPos;

    [SerializeField] private InputActionReference _mousePositionReference;

    [SerializeField] EventReference _pickupMine;
    [SerializeField] EventReference _launchMine;

    private bool _hasGrabbed;

    private void Awake()
    {
        _defaultSprite = _spriteRenderer.sprite;
    }

    private void Start()
    {
        InputManager.Instance.OnLaunched += BeginCharge;
        InputManager.Instance.OnLaunchReleased += ReleaseLaunch;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnLaunched -= BeginCharge;
        InputManager.Instance.OnLaunchReleased -= ReleaseLaunch;
    }

    private void Update()
    {
        AimAtMouse();

        if (!_isCharging || _currentlyHeldMine == null) return;

        _chargeTime = Mathf.Min(_chargeTime + Time.deltaTime, _maxChargeTime);

        float chargeRatio = _chargeTime / _maxChargeTime;
        float shakeAmount = chargeRatio * _maxShakeAmount;
        float offsetX = Mathf.Sin(Time.time * _shakeSpeed) * shakeAmount;
        float offsetY = Mathf.Cos(Time.time * _shakeSpeed * 1.3f) * shakeAmount;
        _currentlyHeldMine.transform.localPosition = _heldMineBaseLocalPos + new Vector3(offsetX, offsetY, 0f);
    }

    private Vector2 _aimDirection;
    private void AimAtMouse()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, -Camera.main.transform.position.z));
        Vector2 direction = (Vector2)(mouseWorld - _rotationPosition.position);
        float angleRad = Mathf.Atan2(direction.y, direction.x);
        float angleDeg = angleRad * Mathf.Rad2Deg;
        _rotationPosition.rotation = Quaternion.Euler(0f, 0f, angleDeg);
        _aimDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private void BeginCharge()
    {
        if (_currentlyHeldMine == null) return;
        _isCharging = true;
        _chargeTime = 0f;
    }

    private void ReleaseLaunch()
    {
        if (_currentlyHeldMine == null) return;

        float chargeRatio = _chargeTime / _maxChargeTime;
        float force = Mathf.Lerp(_minLaunchForce, _maxLaunchForce, chargeRatio);

        // Reset mine position before launch so it doesn't fire from a shaken offset
        _currentlyHeldMine.transform.localPosition = _heldMineBaseLocalPos;
        _currentlyHeldMine.transform.parent = null;
        _currentlyHeldMine.Rb.bodyType = RigidbodyType2D.Dynamic;
        _currentlyHeldMine.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        _currentlyHeldMine.Rb.AddForce(_aimDirection * force * Time.deltaTime, ForceMode2D.Impulse);

        _spriteRenderer.sprite = _defaultSprite;
        _currentlyHeldMine = null;
        _isCharging = false;
        _chargeTime = 0f;

        RuntimeManager.PlayOneShot(_launchMine);

        StartCoroutine(HandleCooldown());
    }

    IEnumerator HandleCooldown()
    {
        _isCoolingDown = true;
        yield return new WaitForSeconds(.2f);
        _isCoolingDown = false;
    }

    public bool TryHoldMine(Mine mine)
    {
        if (_currentlyHeldMine != null) return false;
        _currentlyHeldMine = mine;
        RuntimeManager.PlayOneShot(_pickupMine);

        if (!_hasGrabbed)
        {
            PlayerTalker.Instance.ShowText("It looks like I've got a bomb, hold space.", 5);
            _hasGrabbed = true;
        }

        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCoolingDown) return;
        if (other.gameObject.TryGetComponent<Mine>(out var mine))
        {
            if (!TryHoldMine(mine)) return;
            mine.transform.parent = _holdPosition;
            mine.transform.localPosition = Vector3.zero;
            _heldMineBaseLocalPos = Vector3.zero;
            mine.Rb.bodyType = RigidbodyType2D.Kinematic;
            mine.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            _spriteRenderer.sprite = _bombHoldingSprite;
        }
    }
}