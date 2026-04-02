using UnityEditor;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State _currentState;

    [SerializeField] private Transform _transform;
    public Transform Transform => _transform;

    [SerializeField] private Transform _popupDropzoneTransform;
    public Transform PopupDropzone => _popupDropzoneTransform;

    [SerializeField] private int _RoamingStateChance = 50;
    private int _OriginalRoamingStateChance;
    [SerializeField] private int _ClickingBombsStateChance = 25;
    private int _OriginalClickingBombsStateChance;
    [SerializeField] private int _DraggingStateChance = 25;
    private int _OriginalDraggingStateChance;
    [SerializeField] private int _CustomStateChance = 0;
    private int _OriginalCustomStateChance;

    private float PositivityScaler = 0f;

    public void Init(Transform dropzone)
    {
        _popupDropzoneTransform = dropzone;
    }

    public void SetDifficultyScaler(float scaler)
    {
        _RoamingStateChance = Mathf.RoundToInt(_OriginalRoamingStateChance - scaler * 10);
        _CustomStateChance = Mathf.RoundToInt(_OriginalCustomStateChance + scaler * 3);

        PositivityScaler = -scaler;
    }

    public void Start()
    {
        _currentState = new RoamingState(this);
        _currentState.Enter();

        _OriginalClickingBombsStateChance = _ClickingBombsStateChance;
        _OriginalDraggingStateChance = _DraggingStateChance;
        _OriginalRoamingStateChance = _RoamingStateChance;
        _OriginalCustomStateChance = _CustomStateChance;
    }

    public void ChangeState(State newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public State GetNewState()
    {
        float rand = Random.Range(0f, 1f);
        MousePointerState randomChoice;

        //if(rand < 0.33f)
        //    randomChoice = MousePointerState.Roaming;
        //else if (rand < 0.66f)
        //    randomChoice = MousePointerState.ClickingBombs;
        //else
        float totalChance = _RoamingStateChance + _ClickingBombsStateChance + _DraggingStateChance + _CustomStateChance;

        if (rand < _RoamingStateChance / totalChance)
            randomChoice = MousePointerState.Roaming;
        else if (rand < _ClickingBombsStateChance / totalChance)
            randomChoice = MousePointerState.ClickingBombs;
        else
            randomChoice = MousePointerState.Dragging;




        switch (randomChoice)
        {
            case MousePointerState.ClickingBombs:
                return new ClickingBombsState(this);
            case MousePointerState.Dragging:
                return new DraggingState(this);
            case MousePointerState.Roaming:
                return new RoamingState(this);
            default:
                throw new System.NotImplementedException();
        }
        ;
    }

    public void Update()
    {
        _currentState?.Update();
    }
    
    public void StartResettingProgress(ProgressBar progressBar)
    {
        ChangeState(new ResetMouseProgress(this, progressBar));
    }

}
