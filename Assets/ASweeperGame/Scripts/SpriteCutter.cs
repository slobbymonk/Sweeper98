using Clipper2Lib;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCutter : MonoBehaviour
{
    [SerializeField] private Sprite _maskSprite;

    public void Cut(PopupWindow window, int order, Transform bombTransform)
    {
        GameObject maskGO = new GameObject();
        maskGO.transform.position = bombTransform.position;
        maskGO.transform.localScale = bombTransform.localScale;
        maskGO.transform.parent = window.transform;
        SpriteMask mask = maskGO.AddComponent<SpriteMask>();
        transform.parent = null;

        mask.isCustomRangeActive = true;
        mask.sprite = _maskSprite;
        mask.frontSortingOrder = order + 1;
        mask.backSortingOrder = order - 1;
    }
}