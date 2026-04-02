using FMODUnity;
using PrimeTween;
using System;
using System.Collections;
using UnityEngine;

public class StartingScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _trojanPopUp;
    [SerializeField] private GameObject _mineSweeper;
    [SerializeField] private GameObject _devsPopUp;
    [SerializeField] private GameObject _arrow1PU;
    [SerializeField] private GameObject _arrow2PU;
    [SerializeField] private GameObject _progressBar;
    [SerializeField] private GameObject[] _thingsThatArentInGameScene;

    [SerializeField] private EventReference _introSound;
    [SerializeField] private EventReference _startupSound;

    [SerializeField] private EventReference _popUpSound;

    public Action OnWindowsPopUp;

    private void Start()
    {
        _trojanPopUp.SetActive(false);
        _devsPopUp.SetActive(false);
        _arrow1PU.SetActive(false);
        _arrow2PU.SetActive(false);
        OnIntroScreenStart();
    }

    void OnIntroScreenStart()
    {
        StartCoroutine(IntroDelay());
        RuntimeManager.PlayOneShot(_introSound);
    }

    private IEnumerator IntroDelay()
    {
        yield return new WaitForSeconds(5f);
        OnDesktopStart();
    }

    void OnDesktopStart()
    {
        RuntimeManager.PlayOneShot(_startupSound);
        StartCoroutine(PopUpDelay());
    }

    public void TrojanHorseButton()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {

        _progressBar.SetActive(true);
        Vector2 scale = _progressBar.transform.localScale;
        _progressBar.transform.localScale = scale * .5f;
        Tween.Scale(_progressBar.transform, scale, .2f, Ease.OutBounce);

        for (int i = 0; i < _thingsThatArentInGameScene.Length; i++)
        {
            Tween.Scale(_thingsThatArentInGameScene[i].transform, Vector3.zero, .2f, Ease.InBounce);
        }

        yield return new WaitForSeconds(1);
        
        GameStateManager.Instance.ChangeState(GameStateManager.GameState.Playing);
    }

    private IEnumerator PopUpDelay()
    {
        yield return new WaitForSeconds(0.7f);

        OnWindowsPopUp?.Invoke();
        _trojanPopUp.SetActive(true);

        Vector2 scale = _trojanPopUp.transform.localScale;
        _trojanPopUp.transform.localScale = scale * .5f;
        Tween.Scale(_trojanPopUp.transform, scale, .2f, Ease.OutBounce);

        _mineSweeper.SetActive(true);
        scale = _mineSweeper.transform.localScale;
        _mineSweeper.transform.localScale = scale * .5f;
        Tween.Scale(_mineSweeper.transform, scale, .2f, Ease.OutBounce);

        RuntimeManager.PlayOneShot(_popUpSound);

        yield return new WaitForSeconds(0.6f);

        _devsPopUp.SetActive(true);
        RuntimeManager.PlayOneShot(_popUpSound);

        yield return new WaitForSeconds(0.4f);
        _arrow1PU.SetActive(true);
        RuntimeManager.PlayOneShot(_popUpSound);

        yield return new WaitForSeconds(0.6f);
        _arrow2PU.SetActive(true);
        RuntimeManager.PlayOneShot(_popUpSound);
    }
}
