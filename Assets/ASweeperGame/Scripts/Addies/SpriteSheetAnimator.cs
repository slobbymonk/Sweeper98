using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public enum SpriteAnimatorType
{
    SpriteRenderer,
    SpriteMask,
    Image
}
public class SpriteSheetAnimator : MonoBehaviour
{
    public SpriteAnimation CurrentAnimation { get { return _currentAnimation; } }
    [SerializeField] private SpriteAnimation _currentAnimation;
    [SerializeField] private SpriteAnimatorType _type = SpriteAnimatorType.SpriteRenderer;

    private SpriteRenderer _spriteRenderer;
    private SpriteMask _spriteMask;
    private Image _image;

    private int _currentFrame = 0;
    private float _frameTimer = 0f;

    [SerializeField] private bool _loop = true;
    private enum EndAnimationType
    {
        None,
        Disable,
        Destroy
    }
    [SerializeField] private EndAnimationType _endAnimationType;

    public Action OnAnimationEnded;

    private void Awake()
    {
        switch (_type)
        {
            case SpriteAnimatorType.SpriteRenderer:
                _spriteRenderer = GetComponent<SpriteRenderer>();
                Assert.IsNotNull(_spriteRenderer, $"No SpriteRenderer component was found: <SpriteSheetAnimator> on {gameObject.name}.");
                break;
            case SpriteAnimatorType.SpriteMask:
                _spriteMask = GetComponent<SpriteMask>();
                Assert.IsNotNull(_spriteMask, $"No SpriteMask component was found: <SpriteSheetAnimator> on {gameObject.name}.");
                break;
            case SpriteAnimatorType.Image:
                _image = GetComponent<Image>();
                Assert.IsNotNull(_image, $"No Image component was found: <SpriteSheetAnimator> on {gameObject.name}.");
                break;
        }
    }

    private void Start()
    {
        if (_currentAnimation != null) SetSprite(0);
    }
    private void Update()
    {
        if (_currentAnimation == null || _currentAnimation.Frames.Length == 0)
            return;

        _frameTimer += Time.deltaTime;
        
        if (_frameTimer >= _currentAnimation.Frames[_currentFrame].Duration)
        {
            _frameTimer = 0f;
            _currentFrame++;

            if (_currentFrame >= _currentAnimation.Frames.Length)
            {
                if (_loop)
                {
                    _currentFrame = 0;
                }
                else
                {
                    _currentFrame = _currentAnimation.Frames.Length - 1;
                    OnAnimationEnded?.Invoke();
                    switch (_endAnimationType)
                    {
                        case EndAnimationType.Disable:
                            gameObject.SetActive(false);
                            break;
                        case EndAnimationType.Destroy:
                            Destroy(gameObject);
                            break;
                    }
                    return;
                }
            }

            SetSprite(_currentFrame);
        }
    }

    public void SetAnimation(SpriteAnimation newFrameKeeper)
    {
        _currentAnimation = newFrameKeeper;
        _currentFrame = 0;
        _frameTimer = 0f;
        SetSprite(0);
    }

    private void SetSprite(int frame)
    {
        switch (_type)
        {
            case SpriteAnimatorType.SpriteRenderer:
                _spriteRenderer.sprite = _currentAnimation.Frames[frame].Sprite;
                break;
            case SpriteAnimatorType.SpriteMask:
                _spriteMask.sprite = _currentAnimation.Frames[frame].Sprite;
                break;
            case SpriteAnimatorType.Image:
                _image.sprite = _currentAnimation.Frames[frame].Sprite;
                break;
        }
    }
}