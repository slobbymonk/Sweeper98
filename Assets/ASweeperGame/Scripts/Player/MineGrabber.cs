using UnityEngine;

public class MineGrabber : MonoBehaviour
{
    private Mine _currentlyHeldMine;

    [SerializeField] private Sprite _bombHoldingSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Sprite _defaultSprite;

    private void Awake()
    {
        _defaultSprite = _spriteRenderer.sprite;
    }

    void LaunchBomb()
    {
        if (_currentlyHeldMine == null) return;

        // Launch logic

        _spriteRenderer.sprite = _defaultSprite;
        _currentlyHeldMine = null;
    }

    public bool TryHoldMine(Mine mine)
    {
        if(mine != null) return false;
        _currentlyHeldMine = mine;
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Mine>(out var mine))
        {
            if (!TryHoldMine(mine)) return;
            mine.transform.parent = transform;
            _spriteRenderer.sprite = _bombHoldingSprite;
        }
    }
}
