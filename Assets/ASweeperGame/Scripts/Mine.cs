using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mine : MonoBehaviour
{
    private CircleCutter _circleCutter;
    private SpriteCutter _spriteCutter;
    [SerializeField] float _circleRadius = 1f;
    [SerializeField] Transform _explosionRange;
    [SerializeField] GameObject _explosion;

    public Rigidbody2D Rb { get; private set; }

    private void Awake()
    {
        _circleCutter = GetComponent<CircleCutter>();
        _spriteCutter = GetComponent<SpriteCutter>();
        Rb = GetComponent<Rigidbody2D>();
    }

    [Button]
    public void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _circleRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent<PlayerController>(out var player))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if (colliders[i].gameObject.TryGetComponent<PopupWindowCloseButton>(out var closeButton))
            {
                Destroy(closeButton.Window.gameObject);
                continue;
            }

            if (!colliders[i].gameObject.TryGetComponent<PopupWindow>(out var window)) continue;

            _circleCutter.Cut(window, _circleRadius);
            int renderingOrder = window.RenderingOrder;
            _spriteCutter.Cut(window, renderingOrder, _explosionRange);
        }

        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<PopupWindow>(out var window) 
            || collision.gameObject.TryGetComponent<Mine>(out var _))
        {
            Explode();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, _circleRadius);
    }
}
