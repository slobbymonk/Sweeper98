using PrimeTween;
using UnityEngine;

public class OSMenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform _OSMenu;
    [SerializeField] private RectTransform _target;
    private bool _IsOpen = false;

    public void OpenMenu()
    {
        if (_IsOpen)
        {
            CloseMenu();
            _IsOpen = false;
        }
        else
        {
            Tween.UIAnchoredPosition(_OSMenu, endValue: new Vector3(71.4f, 118f, 0f), duration: 0.5f, ease: Ease.OutBack);
            _IsOpen = true;

        }
    }

    public void CloseMenu()
    {
        Tween.UIAnchoredPosition(_OSMenu, endValue: new Vector3(71.4f, -110f, 0f), duration: 0.5f, ease: Ease.OutBack);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
