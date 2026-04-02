using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _winScreen;
    public enum GameState { MainMenu, Playing, GameOver, Win }

    public GameState CurrentState { get; private set; }
    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeState(string newState)
    {
        if (Enum.TryParse(newState, out GameState parsedState))
        {
            ChangeState(parsedState);
        }
        else
        {
            Debug.LogError($"Invalid game state: {newState}");
        }
    }
    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
        HandleState(newState);
    }

    private void HandleState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                SceneManager.LoadScene("TitleScene");
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                SceneManager.LoadScene("PlayingScene");
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                GameObject.Instantiate(_gameOverScreen);
                break;
            case GameState.Win:
                GameObject.Instantiate(_winScreen);
                break;
        }
        }

    }