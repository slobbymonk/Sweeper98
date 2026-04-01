using UnityEngine;

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
        _targetBomb = null;
    }
}


