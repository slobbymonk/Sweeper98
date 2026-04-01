using UnityEngine;
using FMODUnity;

public class Mine : MonoBehaviour
{
    private CircleCutter _circleCutter;
    private SpriteCutter _spriteCutter;
    [SerializeField] float _circleRadius = 1f;
    [SerializeField] Transform _explosionRange;
    [SerializeField] GameObject _explosion;
    [SerializeField] EventReference _explosionSound;
    [SerializeField] EventReference _bounceOffEdgeSound;

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
            if (colliders[i].gameObject.TryGetComponent<PlayerHealth>(out var playerHealth))
            {
                playerHealth.Die();
            }
            if (colliders[i].gameObject.TryGetComponent<PopupWindowCloseButton>(out var closeButton))
            {
                closeButton.Window.CloseWindow();
                continue;
            }
            if (colliders[i].gameObject.TryGetComponent<Bug>(out var bug))
            {
                bug.Die();
                continue;
            }
        

            if (!colliders[i].gameObject.TryGetComponent<PopupWindow>(out var window)) continue;

            _circleCutter.Cut(window, _circleRadius);
            int renderingOrder = window.RenderingOrder;
            _spriteCutter.Cut(window, renderingOrder, _explosionRange);
        }

        ScreenShake.Instance.ShakeTarget("BombExplosion", .3f, .3f);
        Instantiate(_explosion, transform.position, Quaternion.identity);
        RuntimeManager.PlayOneShot(_explosionSound);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ScreenEdge"))
        {
            RuntimeManager.PlayOneShot(_bounceOffEdgeSound);
            return;
        }


        if (collision.gameObject.TryGetComponent<PopupWindow>(out var window)
            || collision.gameObject.TryGetComponent<Mine>(out var _)
            || collision.gameObject.TryGetComponent<Bug>(out var _))
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
