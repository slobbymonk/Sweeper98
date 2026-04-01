using FMODUnity;
using System.Collections;
using UnityEngine;

public class GameOverLogic : MonoBehaviour
{
    [SerializeField] private GameObject _blueScreenOfDeath;
    [SerializeField] private GameObject[] _errors;
    [SerializeField] private EventReference _errorPopupSound;

    private void Start()
    {
        StartCoroutine(HandleAnimation());
    }

    IEnumerator HandleAnimation()
    {
        for (int i = 0; i < _errors.Length; i++)
        {
            _errors[i].SetActive(true);
            RuntimeManager.PlayOneShot(_errorPopupSound);
            if(i == 0) yield return new WaitForSecondsRealtime(1);
            if(i == 1) yield return new WaitForSecondsRealtime(.5f);
            if(i == 2) yield return new WaitForSecondsRealtime(.2f);
            else yield return new WaitForSecondsRealtime(.05f);
        }

        _blueScreenOfDeath.SetActive(true);
    }
}
