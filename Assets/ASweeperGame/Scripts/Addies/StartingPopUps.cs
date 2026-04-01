using UnityEngine;
using FMODUnity;
using UnityEngine.Android;

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

    public void TrojanHorseButton()
    {
        StartGame();
    }

    public void TrojanHorseClose()
    {
        _trojanClosePopUps[_currentPopUpIndex].gameObject.SetActive(true);
        _currentPopUpIndex += 1;
        RuntimeManager.PlayOneShot(_popUpSound);
    }

    void StartGame()
    {
       GameStateManager.Instance.ChangeState(GameStateManager.GameState.Playing);
    }
}
