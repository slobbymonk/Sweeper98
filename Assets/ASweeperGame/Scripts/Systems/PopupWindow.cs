using FMODUnity;
using System;
using UnityEngine;
using PrimeTween;
using System.Collections;

public class PopupWindow : MonoBehaviour, IDraggable, IDestroyable
{
    public bool HasBeenDragged { get; set; }
    public bool _hasBeenDragged = false;
    public Action<PopupWindow> OnDestroyed { get; set; }
    public Action<IDraggable> OnGrabbed { get; set; }

    public BoxCollider2D BoxCollider {  get; private set; }
    public int RenderingOrder { get; private set; }

    [SerializeField] private SpriteRenderer _bgSpriteRenderer;
    public SpriteRenderer BGSpriteRenderer => _bgSpriteRenderer;
    [SerializeField] private SpriteRenderer _contentSpriteRenderer;
    public SpriteRenderer ContentSpriteRenderer => _contentSpriteRenderer;

    [SerializeField] private SpriteRenderer _closeButton;
    [SerializeField] private Canvas _textCanvas;

    [SerializeField] private EventReference _spawnSound;
    [SerializeField] private EventReference _closeSound;

    void Awake()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        Vector2 windowScale = transform.localScale;
        transform.localScale = windowScale * .5f;
        Tween.Scale(transform, windowScale, .2f, Ease.OutBounce);
        SetWindowToHighestLevel();

        RuntimeManager.PlayOneShot(_spawnSound);
    }

    private void SetWindowToHighestLevel()
    {
        RenderingOrder = MaskOutManager.Instance.GetRenderingOrder();
        _closeButton.sortingOrder = RenderingOrder + 1;
        _contentSpriteRenderer.sortingOrder = RenderingOrder + 1;
        _textCanvas.sortingOrder = RenderingOrder + 1;
        _bgSpriteRenderer.sortingOrder = RenderingOrder;
        SpriteMask[] masks = gameObject.GetComponentsInChildren<SpriteMask>();
        for (int i = 0; i < masks.Length; i++)
        {
            masks[i].frontSortingOrder = RenderingOrder + 1;
            masks[i].backSortingOrder = RenderingOrder - 1;
        }
    }

    public void CloseWindow()
    {
        StartCoroutine(CloseWindowAnimation());
    }
    IEnumerator CloseWindowAnimation()
    {
        yield return Tween.Scale(transform, Vector3.zero, .5f, Ease.InBounce);
        
        RuntimeManager.PlayOneShot(_closeSound);
        OnDestroyed?.Invoke(this);

        Destroy(gameObject);
    }

    public void GrabLogic()
    {
        SetWindowToHighestLevel();
        OnGrabbed?.Invoke(this);
    }
}