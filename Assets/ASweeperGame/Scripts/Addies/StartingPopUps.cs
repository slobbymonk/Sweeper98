using UnityEngine;
using FMODUnity;
using UnityEngine.Android;
using PrimeTween;

public class StartingPopUps : MonoBehaviour
{
    [SerializeField] private EventReference _popUpSound;
    [SerializeField] private GameObject[] _trojanClosePopUps;
    private int _currentPopUpIndex;

    void Awake()
    {
        _currentPopUpIndex = 0;
        CloseAllPopUps();
    }

    void CloseAllPopUps()
    {
        foreach (GameObject go in _trojanClosePopUps)
        {
            go.SetActive(false);
        }
    }

    public void TrojanHorseClose()
    {
        _trojanClosePopUps[_currentPopUpIndex].gameObject.SetActive(true);
        Vector2 scale = _trojanClosePopUps[_currentPopUpIndex].transform.localScale;
        _trojanClosePopUps[_currentPopUpIndex].transform.localScale = scale * .5f;
        Tween.Scale(_trojanClosePopUps[_currentPopUpIndex].transform, scale, .2f, Ease.OutBounce);
        _currentPopUpIndex += 1;
        RuntimeManager.PlayOneShot(_popUpSound);
    }
}
