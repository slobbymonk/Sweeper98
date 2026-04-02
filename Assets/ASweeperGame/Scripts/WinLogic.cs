using PrimeTween;
using System.Collections;
using UnityEngine;

public class WinLogic : MonoBehaviour
{
    [SerializeField] private GameObject _winPopup;

    private void Awake()
    {
        StartCoroutine(HandleAnimation());
    }
    IEnumerator HandleAnimation()
    {
        PopupWindowManager.Instance.CloseAllWindows();
        PopupWindowManager.Instance.CanSpawn = false;
        FindAnyObjectByType<BombManager>().CanSpawn = false;
        FindAnyObjectByType<PassiveBugSpawner>().CanSpawn = false;
        FindAnyObjectByType<ProgressBar>().gameObject.SetActive(false);

        yield return new WaitForEndOfFrame();
        foreach (var bug in FindObjectsOfType<Bug>())
        {
            bug.gameObject.SetActive(false);
        }
        foreach (var bug in FindObjectsOfType<StateMachine>())
        {
            bug.transform.root.gameObject.SetActive(false);
        }

        yield return new WaitForSecondsRealtime(2);

        _winPopup.SetActive(true);
        Vector2 scale = _winPopup.transform.localScale;
        _winPopup.transform.localScale = scale * .5f;
        yield return Tween.Scale(_winPopup.transform, scale, .2f, Ease.OutBounce, useUnscaledTime: true);
    }

    [Button]
    public void CloseGame()
    {
        StartCoroutine(HandleClosingGame());
    }
    IEnumerator HandleClosingGame()
    {
        Application.Quit();

        yield return null;
    }
}
