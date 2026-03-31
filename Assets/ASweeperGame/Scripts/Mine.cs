using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private CircleCutter _circleCutter;
    private SpriteCutter _spriteCutter;
    [SerializeField] float _circleRadius = 1f;
    [SerializeField] Transform _explosionRange;

    private void Awake()
    {
        _circleCutter = GetComponent<CircleCutter>();
        _spriteCutter = GetComponent<SpriteCutter>();
    }

    [Button]
    public void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _circleRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (!colliders[i].gameObject.TryGetComponent<PopupWindow>(out var window)) continue;

            _circleCutter.Cut(window, _circleRadius);
            int renderingOrder = window.RenderingOrder;
            _spriteCutter.Cut(window, renderingOrder, _explosionRange);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, _circleRadius);
    }
}
