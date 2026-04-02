using UnityEngine;
using FMODUnity;
using TMPro;
using PrimeTween;

public enum MineState
{
    InMineSweeper,
    Interactible
}

public class Mine : MonoBehaviour
{
    private CircleCutter _circleCutter;
    private SpriteCutter _spriteCutter;
    [SerializeField] float _circleRadius = 1f;
    [SerializeField] float _playerKillRadius = 1f;
    [SerializeField] Transform _explosionRange;
    [SerializeField] GameObject _explosion;
    [SerializeField] EventReference _explosionSound;
    [SerializeField] EventReference _bounceOffEdgeSound;

    [SerializeField] SpriteRenderer _spriteRenderer;
    public bool IsInteractible = false;
    [SerializeField] int _mineSweeperLayer;
    [SerializeField] int _interactibleLayer;
    [SerializeField] Sprite _interactibleMine;
    [SerializeField] Sprite _uninteractibleMine;

    public Rigidbody2D Rb { get; private set; }


    private bool _hasExploded = false;

    private void Awake()
    {
        _circleCutter = GetComponent<CircleCutter>();
        _spriteCutter = GetComponent<SpriteCutter>();
        Rb = GetComponent<Rigidbody2D>();

        gameObject.layer = _mineSweeperLayer;
        _spriteRenderer.sprite = _uninteractibleMine;
    }

    [Button]
    public void Explode()
    {
        if (_hasExploded) return;
        _hasExploded = true;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _circleRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent<TMP_Text>(out var text))
            {
                Destroy(text.gameObject);
            }
            if (colliders[i].gameObject.TryGetComponent<PlayerHealth>(out var health))
            {
                if (Vector3.Distance(transform.position, health.transform.position) > _playerKillRadius) continue;
                health.Die();
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
            if (colliders[i].gameObject.TryGetComponent<Mine>(out var mine))
            {
                mine.Explode();
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

    public void GrabMine()
    {
        IsInteractible = true;
        _spriteRenderer.sprite = _interactibleMine;
        gameObject.layer = _interactibleLayer;
        Tween.PunchScale(transform, Vector3.one * 1.2f, .1f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, _circleRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _playerKillRadius);
    }
}
