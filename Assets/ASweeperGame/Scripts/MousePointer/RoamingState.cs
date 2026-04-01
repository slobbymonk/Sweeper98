using UnityEngine;

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
            int attempts = 0;
            do
            {
                attempts++;
                randomPoint = new Vector2(
                    Random.Range(_screenLeftBottom.x, _screenBoundsTopRight.x),
                    Random.Range(_screenLeftBottom.y, _screenBoundsTopRight.y));
            } while (Vector2.Distance(lastPoint, randomPoint) < 8f && attempts < 50);
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


