using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum MousePointerState { Roaming, Dragging, ClickingBombs }
public abstract class State
{
    protected StateMachine stateMachine;

    private Vector2 _velocity;

    public State(StateMachine stateMachine) { this.stateMachine = stateMachine; }

    protected bool MoveToward(Vector2 target, float smoothTime, float maxSpeed, float arrivalThreshold)
    {
        Vector2 current = stateMachine.Transform.position;
        Vector2 newPos = Vector2.SmoothDamp(current, target, ref _velocity, smoothTime, maxSpeed);
        stateMachine.Transform.position = newPos;

        if (Vector2.Distance(newPos, target) < arrivalThreshold)
        {
            _velocity = Vector2.zero;
            return true;
        }
        return false;
    }

    protected void ResetVelocity() => _velocity = Vector2.zero;


    public virtual void Enter() 
    {
    }
    public virtual void Update() 
    { 
    
    }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}

public class RoamingState : State
{
    private Vector2[] _roamingPoints;
    private int _currentPointIndex;
    private const float _arrivalThreshold = 0.6f;
    private const float _smoothTime = 0.5f;
    private const float _maxSpeed = 3.5f;
    private Vector2 _screenLeftBottom = new Vector2(-8.2f, -4.3f);
    private Vector2 _screenBoundsTopRight = new Vector2(8.2f, 4.3f);

    private Vector2 velocity; // SmoothDamp tracks this internally

    public RoamingState(StateMachine mousePointer) : base(mousePointer) { }

    public override void Enter()
    {
        int numberOfRoamingPoints = Random.Range(1, 3);
        _roamingPoints = new Vector2[numberOfRoamingPoints];
        for (int i = 0; i < numberOfRoamingPoints; i++)
        {
            Vector2 lastPoint = i == 0 ? (Vector2)stateMachine.Transform.position : _roamingPoints[i - 1];
            Vector2 randomPoint;
            do
            {
                randomPoint = new Vector2(
                    Random.Range(_screenLeftBottom.x, _screenBoundsTopRight.x),
                    Random.Range(_screenLeftBottom.y, _screenBoundsTopRight.y));
            } while (Vector2.Distance(lastPoint, randomPoint) < 8f);
            _roamingPoints[i] = randomPoint;
        }

        velocity = Vector2.zero;
    }

    public override void Update()
    {
        if (_roamingPoints == null || _roamingPoints.Length == 0)
            stateMachine.ChangeState(stateMachine.GetNewState());

        bool arrived = MoveToward(_roamingPoints[_currentPointIndex], _smoothTime, _maxSpeed, _arrivalThreshold);

        if (arrived)
        {
            _currentPointIndex++;
            if (_currentPointIndex >= _roamingPoints.Length)
                stateMachine.ChangeState(stateMachine.GetNewState());
        }
    }

    public override void Exit()
    {
        _roamingPoints = null;
        velocity = Vector2.zero;
    }
}

public class DraggingState : State
{
    public DraggingState(StateMachine mousePointer) : base(mousePointer) { }

    private PopupWindow _targetPopup = null;
    private bool _holdsPopup = false;
    private bool _skipToNext = false;
    private Vector2 _popupDropoffPoint;

    public override void Enter()
    {
        PopupWindow[] windows = GameObject.FindObjectsByType<PopupWindow>(FindObjectsSortMode.None);
        List<PopupWindow> undraggedWindows = new List<PopupWindow>();

        foreach (PopupWindow window in windows)
        {
            if (!window.HasBeenDragged)
                undraggedWindows.Add(window);
        }

        if (undraggedWindows.Count == 0)
        {
            _skipToNext = true; // defer — never call ChangeState from Enter
            return;
        }

        _targetPopup = undraggedWindows[Random.Range(0, undraggedWindows.Count)];
    }

    public override void Update()
    {
        if (_skipToNext)
        {
            _skipToNext = false;
            stateMachine.ChangeState(stateMachine.GetNewState());
            return;
        }

        if (_targetPopup == null)
            stateMachine.ChangeState(stateMachine.GetNewState());

        if (!_holdsPopup)
        {
            bool arrived = MoveToward(_targetPopup.transform.position, 0.1f, 10f, 0.5f);
            if (arrived)
            {
                _holdsPopup = true;
                FindPopupDropzone();
                _targetPopup.transform.SetParent(stateMachine.Transform);
            }
        }
        else
        {
            if (MoveToward(_popupDropoffPoint, 0.1f, 2f, 0.5f))
            {
                _targetPopup.HasBeenDragged = true;
                _targetPopup.transform.SetParent(null);
                stateMachine.ChangeState(stateMachine.GetNewState());
            }
        }
    }

    public override void Exit() { }

    private void FindPopupDropzone()
    {
        _popupDropoffPoint = new Vector2(
            Random.Range(stateMachine.PopupDropzone.position.x - stateMachine.PopupDropzone.localScale.x / 2,
                         stateMachine.PopupDropzone.position.x + stateMachine.PopupDropzone.localScale.x / 2),
            Random.Range(stateMachine.PopupDropzone.position.y - stateMachine.PopupDropzone.localScale.y / 2,
                         stateMachine.PopupDropzone.position.y + stateMachine.PopupDropzone.localScale.y / 2));
    }
}

public class ClickingBombsState : State
{
    Mine _targetBomb;
    public ClickingBombsState(StateMachine mousePointer) : base(mousePointer) { }
    public override void Enter()
    {
        Mine[] bombs = GameObject.FindObjectsByType<Mine>(FindObjectsSortMode.None);
        int randomIndex = Random.Range(0, bombs.Length);
        _targetBomb = bombs[randomIndex];

    }
    public override void Update()
    {
        if(_targetBomb == null)
            stateMachine.ChangeState(stateMachine.GetNewState());


        if (MoveToward(_targetBomb.transform.position, 0.1f, 2f, 0.5f))
        {
            _targetBomb.Explode();
            stateMachine.ChangeState(stateMachine.GetNewState());
        }
    }
    public override void Exit()
    {
        // Code to execute when exiting clicking bombs state
    }
}



public class StateMachine : MonoBehaviour
{
    private State _currentState;

    [SerializeField] private Transform _transform;
    public Transform Transform => _transform;

    [SerializeField] private Transform _popupDropzoneTransform;
    public Transform PopupDropzone => _popupDropzoneTransform;

    public void Init(Transform dropzone)
    {
        _popupDropzoneTransform = dropzone;
    }

    public void Start()
    {
        _currentState = new RoamingState(this);
        _currentState.Enter();
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

        if(rand < 0.7f)
            randomChoice = MousePointerState.Roaming;
        else if (rand < 0.90f)
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
        };
    }

    public void Update()
    {
        _currentState?.Update();
    }
    public void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

}


