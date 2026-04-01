using PrimeTween;
using System;
using UnityEngine;

public class PlayerControllerEffects : MonoBehaviour
{
    [SerializeField] private float _maxRotationOffset;
    [SerializeField] private Transform _visuals;

    private PlayerController _playerController;
    private MineGrabber _mineGrabber;

    private bool _isLookingRight = true;
    private Vector3 _originalScale;
    private Vector3 _previousPosition;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _mineGrabber = GetComponent<MineGrabber>();
        _originalScale = _visuals.localScale;

        _mineGrabber.OnGrabbedMine += HandleMineGrabbed;

        Vector2 scale = transform.localScale;
        transform.localScale = scale * .5f;
        Tween.Scale(transform, scale, .2f, Ease.OutBounce);
    }

    private void OnDestroy()
    {
        _mineGrabber.OnGrabbedMine -= HandleMineGrabbed;
    }

    private void HandleMineGrabbed()
    {
        Tween.PunchScale(_visuals, Vector3.one * 1.2f, .1f);
    }

    private void Update()
    {
        Vector2 moveDirection = transform.position - _previousPosition;
        float moveSpeed = _playerController.Rigidbody.linearVelocity.magnitude;
        float rotation = Mathf.Lerp(0, _maxRotationOffset, moveSpeed / _playerController.MaxSpeed);
        if(!_isLookingRight) rotation = -rotation;

        _visuals.localEulerAngles = new Vector3(_visuals.localRotation.x, _visuals.localRotation.y, rotation);
        if (moveDirection.x < -0.1f && !_isLookingRight)
        {
            _visuals.localScale = _originalScale;
            _isLookingRight = true;
        }
        else if(moveDirection.x > 0.1f && _isLookingRight)
        {
            _visuals.localScale = new Vector3(-_originalScale.x, _originalScale.y, _originalScale.z);
            _isLookingRight = false;
        }
        _previousPosition = transform.position;
    }
}
