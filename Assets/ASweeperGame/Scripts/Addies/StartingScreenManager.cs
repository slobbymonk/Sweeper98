using FMODUnity;
using System.Collections;
using UnityEngine;

public class StartingScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _trojanPopUp;
    [SerializeField] private GameObject _devsPopUp;

    [SerializeField] private EventReference _introSound;
    [SerializeField] private EventReference _startupSound;

    [SerializeField] private EventReference _popUpSound;

    private void Start()
    {
        _trojanPopUp.SetActive(false);
        _devsPopUp.SetActive(false);
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

    private IEnumerator PopUpDelay()
    {
        yield return new WaitForSeconds(0.7f);

        _trojanPopUp.SetActive(true);
        RuntimeManager.PlayOneShot(_popUpSound);

        yield return new WaitForSeconds(0.6f);

        _devsPopUp.SetActive(true);
        RuntimeManager.PlayOneShot(_popUpSound);
    }
}
