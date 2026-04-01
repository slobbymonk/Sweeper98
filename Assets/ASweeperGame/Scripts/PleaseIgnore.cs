using PrimeTween;
using UnityEngine;

public class PleaseIgnore : MonoBehaviour
{
    private void Awake()
    {
        Vector2 scale = transform.localScale;
        transform.localScale = scale * .5f;
        Tween.Scale(transform, scale, .2f, Ease.OutBounce);
    }
}
