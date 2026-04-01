using PrimeTween;
using UnityEngine;

public class OSMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _OSMenu;
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
            Tween.Scale(_OSMenu.transform, endValue: new Vector3(1f, 1f, 1f), duration: 0.5f, ease: Ease.OutBack);
            _IsOpen = true;

        }
    }

    public void CloseMenu()
    {
        Tween.Scale(_OSMenu.transform, endValue: new Vector3(0f, 0f, 0f), duration: 0.5f, ease: Ease.OutBack);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
