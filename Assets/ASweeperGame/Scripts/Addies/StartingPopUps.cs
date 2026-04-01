using UnityEngine;
using UnityEngine.Android;

public class StartingPopUps : MonoBehaviour
{
    [SerializeField] private GameObject[] _trojanClosePopUp;
    [SerializeField] private Transform _initialSpawnPosition;
    [SerializeField] private Vector2 _trojanClosePopUpOffset;
    private int _currentPopUpIndex;
    private Vector2 _lastPopUpPosition;

    void Awake()
    {
        _currentPopUpIndex = 0;
        _lastPopUpPosition = _initialSpawnPosition.transform.position;
    }

    public void TrojanHorseButton()
    {
        StartGame();
    }

    public void TrojanHorseClose()
    {
        if (_currentPopUpIndex > _trojanClosePopUp.Length - 1) return;
        GameObject newPopUp = GameObject.Instantiate(_trojanClosePopUp[_currentPopUpIndex], _lastPopUpPosition + _trojanClosePopUpOffset, Quaternion.identity);

        _lastPopUpPosition = newPopUp.transform.position;
        _currentPopUpIndex++;
    }

    void StartGame()
    {
       GameStateManager.Instance.ChangeState(GameStateManager.GameState.Playing);
    }
}
