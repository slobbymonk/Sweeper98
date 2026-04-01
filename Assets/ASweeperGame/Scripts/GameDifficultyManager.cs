using UnityEngine;

public class GameDifficultyManager : MonoBehaviour
{
    [SerializeField] private float _maxDifficultyScaler = 3f;
    [SerializeField] private float _difficultyIncreaseRate = 0.1f;
    [SerializeField] private float _currentDifficultyScaler = 1f;
    private float _timer = 0;

    void Start()
    {
        
    }

    void Update()
    {
        _timer = _timer + Time.deltaTime;
        if(_timer > 5f && _currentDifficultyScaler < _maxDifficultyScaler)
        {
            _timer = 0;

            _currentDifficultyScaler += _difficultyIncreaseRate;
            ApplyNewDifficulty();
        }
    }

    private void ApplyNewDifficulty()
    {
        StateMachine[] stateMachines = FindObjectsByType<StateMachine>(FindObjectsSortMode.None);
        foreach(StateMachine stateMachine in stateMachines)
        {
            stateMachine.SetDifficultyScaler(_currentDifficultyScaler);
        }

        PassiveBugSpawner[] passiveBugSpawners = FindObjectsByType<PassiveBugSpawner>(FindObjectsSortMode.None);
        foreach(PassiveBugSpawner passiveBugSpawner in passiveBugSpawners)
        {
            passiveBugSpawner.SetDifficultyScaler(_currentDifficultyScaler);
        }

        PopupWindowManager[] popupWindowManagers = FindObjectsByType<PopupWindowManager>(FindObjectsSortMode.None);
        foreach(PopupWindowManager popupWindowManager in popupWindowManagers)
        {
            popupWindowManager.SetDifficultyScaler(_currentDifficultyScaler);
        }

        MousePointerSpawner[] mousePointerSpawners = FindObjectsByType<MousePointerSpawner>(FindObjectsSortMode.None);
        foreach(MousePointerSpawner mousePointerSpawner in mousePointerSpawners)
        {
            mousePointerSpawner.SetDifficultyScalar(_currentDifficultyScaler);
        }
    }
}
