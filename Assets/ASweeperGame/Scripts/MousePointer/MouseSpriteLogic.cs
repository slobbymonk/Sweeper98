using UnityEngine;

public class MouseSpriteLogic : MonoBehaviour
{
    enum MouseState
    {
        Neutral,
        Clickable,
        grabReady,
        grabbing,
    }
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite _neutralSprite;
    [SerializeField] private Sprite _clickableSprite;
    [SerializeField] private Sprite _grabReadeySprite;
    [SerializeField] private Sprite _grabbingSprite;

    private bool _locked = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Clickable") && !_locked)
        {
            spriteRenderer.sprite = _clickableSprite;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Clickable"))
        {
            spriteRenderer.sprite = _neutralSprite;
        }
    }

    public void GrabAnim()
    {
        _locked = true;
        spriteRenderer.sprite = _grabbingSprite;
    }    

    public void StopGrabAnim()
    {
        _locked = false;
        spriteRenderer.sprite = _neutralSprite;
    }

    private void SetMouseState(MouseState state)
    {
        if(_locked) return;

        spriteRenderer.sprite = state switch
        {
            MouseState.Neutral => _neutralSprite,
            MouseState.Clickable => _clickableSprite,
            MouseState.grabReady => _grabReadeySprite,
            MouseState.grabbing => _grabbingSprite,
            _ => spriteRenderer.sprite
        };
    }

}
