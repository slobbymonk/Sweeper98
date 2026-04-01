using UnityEditor;
using UnityEngine;

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

        if(rand < 0.4f)
            randomChoice = MousePointerState.Roaming;
        else if (rand < 0.75f)
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


