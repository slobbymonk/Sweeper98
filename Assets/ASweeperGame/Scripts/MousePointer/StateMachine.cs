using UnityEngine;


public enum MousePointerState { Roaming, Dragging, ClickingBombs }
public abstract class State
{
    protected StateMachine stateMachine;

    public State(StateMachine stateMachine) { this.stateMachine = stateMachine; }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}

public class RoamingState : State
{
    private Vector2[] roamingPoints;
    private int currentPointIndex;
    private const float arrivalThreshold = 0.6f;
    private const float smoothTime = 0.5f;
    private const float maxSpeed = 3.5f;
    private Vector2 screenLeftBottom = new Vector2(-8.2f, -4.3f);
    private Vector2 screenBoundsTopRight = new Vector2(8.2f, 4.3f);

    private Vector2 velocity; // SmoothDamp tracks this internally

    public RoamingState(StateMachine mousePointer) : base(mousePointer) { }

    public override void Enter()
    {
        int numberOfRoamingPoints = Random.Range(1, 3);
        roamingPoints = new Vector2[numberOfRoamingPoints];
        for (int i = 0; i < numberOfRoamingPoints; i++)
        {
            Vector2 lastPoint = i == 0 ? (Vector2)stateMachine.Transform.position : roamingPoints[i - 1];
            Vector2 randomPoint;
            do
            {
                randomPoint = new Vector2(
                    Random.Range(screenLeftBottom.x, screenBoundsTopRight.x),
                    Random.Range(screenLeftBottom.y, screenBoundsTopRight.y));
            } while (Vector2.Distance(lastPoint, randomPoint) < 8f);
            roamingPoints[i] = randomPoint;
        }

        velocity = Vector2.zero;
    }

    public override void Update()
    {
        if (roamingPoints == null || roamingPoints.Length == 0) return;

        Vector2 current = stateMachine.Transform.position;
        Vector2 target = roamingPoints[currentPointIndex];

        Vector2 newPos = Vector2.SmoothDamp(current, target, ref velocity, smoothTime, maxSpeed);
        stateMachine.Transform.position = newPos;

        if (Vector2.Distance(newPos, target) < arrivalThreshold)
        {
            currentPointIndex++;
            velocity = Vector2.zero; // reset so it eases in to the next point
            if (currentPointIndex >= roamingPoints.Length)
                stateMachine.ChangeState(stateMachine.GetNewState());
        }
    }

    public override void Exit()
    {
        roamingPoints = null;
        velocity = Vector2.zero;
    }
}

public class DraggingState : State
{
    public DraggingState(StateMachine mousePointer) : base(mousePointer) { }
    public override void Enter()
    {
        // Code to execute when entering dragging state
    }
    public override void Update()
    {
        // Code to execute while in dragging state
    }
    public override void Exit()
    {
        // Code to execute when exiting dragging state
    }
}

public class ClickingBombsState : State
{
    public ClickingBombsState(StateMachine mousePointer) : base(mousePointer) { }
    public override void Enter()
    {
        // Code to execute when entering clicking bombs state
    }
    public override void Update()
    {
        // Code to execute while in clicking bombs state
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
        MousePointerState randomChoice = (MousePointerState)Random.Range(0, 2);


        return new RoamingState(this);

        //switch(randomChoice)
        //{
        //    case MousePointerState.Roaming: 
        //        return new DraggingState(this);
        //        break;
        //    case MousePointerState.Dragging: 
        //        return new ClickingBombsState(this);
        //        break;
        //    case MousePointerState.ClickingBombs:
        //        returnnew RoamingState(this);
        //        break;
        //    default:
        //        throw new System.NotImplementedException();
        //};
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


