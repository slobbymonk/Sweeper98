using System;
using UnityEngine;

public class PopupWindow : MonoBehaviour, IDraggable, IDestroyable
{
    public bool HasBeenDragged { get; set; }
    public bool _hasBeenDragged = false;
    public Action<PopupWindow> OnDestroyed { get; set; }

    public BoxCollider2D BoxCollider {  get; private set; }
    public int RenderingOrder { get; private set; }

    [SerializeField] private SpriteRenderer _bgSpriteRenderer;
    public SpriteRenderer BGSpriteRenderer => _bgSpriteRenderer;
    [SerializeField] private SpriteRenderer _contentSpriteRenderer;
    public SpriteRenderer ContentSpriteRenderer => _contentSpriteRenderer;

    [SerializeField] private SpriteRenderer _closeButton;

    void Awake()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        RenderingOrder = MaskOutManager.Instance.GetRenderingOrder();
        _closeButton.sortingOrder = RenderingOrder + 1;
        _contentSpriteRenderer.sortingOrder = RenderingOrder + 1;
        _bgSpriteRenderer.sortingOrder = RenderingOrder;
    }

    void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}
