using UnityEngine;

public class PopupWindowCloseButton : MonoBehaviour
{
    public PopupWindow Window { get; private set; }

    private void Awake()
    {
        Window = GetComponentInParent<PopupWindow>();
    }
}
