using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    [SerializeField] private GameObject _gameOverScreen;
    public enum GameState { MainMenu, Playing, GameOver }

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
        if (CurrentState == newState) return;
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
                SceneManager.LoadScene("PlayingScene");
                Time.timeScale = 1f;
                break;
            case GameState.GameOver:
                GameObject.Instantiate(_gameOverScreen);
                Time.timeScale = 0f;
                break;
        }
        }

    }