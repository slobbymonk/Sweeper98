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


