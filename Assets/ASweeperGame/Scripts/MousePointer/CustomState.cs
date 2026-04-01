using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class CustomState : State
{
    public CustomState(StateMachine mousePointer, ICustomInteraction target) : base(mousePointer)
    {
        _target = target;
    }

    private ICustomInteraction _target;
    private ICustomInteraction.InteractionPoint[] _interactionPoints;
    private int _currentPointIndex = 0;
    private float _waitTimer = 0f;
    private bool _isWaiting = false;

    public override void Enter()
    {
        _interactionPoints = _target.GetInteractionPoints();
        _currentPointIndex = 0;
        _waitTimer = 0f;
        _isWaiting = false;
    }

    public override void Update()
    {
        if (_interactionPoints.Length == 0 || _currentPointIndex >= _interactionPoints.Length)
        {
            stateMachine.ChangeState(stateMachine.GetNewState());
            return;
        }

        ICustomInteraction.InteractionPoint currentPoint = _interactionPoints[_currentPointIndex];

        if (_isWaiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                _isWaiting = false;
                _currentPointIndex++;
            }
            return;
        }

        bool arrived = MoveToward(currentPoint.position, 0.5f, currentPoint.speed, currentPoint.arrivalThreshold);
        if (arrived)
        {
            currentPoint.onReached?.Invoke();

            if (currentPoint.waitAfter > 0f)
            {
                _waitTimer = currentPoint.waitAfter;
                _isWaiting = true;
            }
            else
            {
                _currentPointIndex++;
            }
        }
    }

    public override void Exit()
    {
        _waitTimer = 0f;
        _isWaiting = false;
    }
}




