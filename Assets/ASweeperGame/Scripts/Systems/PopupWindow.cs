using System;
using UnityEngine;

public class PopupWindow : MonoBehaviour, IDraggable, IDestroyable
{
    public Action<PopupWindow> OnDestroyed { get; set; }

    public BoxCollider2D BoxCollider {  get; private set; }

    void Awake()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
    }
}
