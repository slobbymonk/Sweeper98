using UnityEngine;

public class ResetMouseProgress : State
{
    ProgressBar _progressBar;
    public ResetMouseProgress(StateMachine stateMachine, ProgressBar progressBar) : base(stateMachine) 
    {
        _progressBar = progressBar;
    }

    private bool _isResetting = false;

    public override void Enter()
    {
    }
    public override void Update()
    {
        if(!_isResetting)
        {
            if(MoveToward(_progressBar.GetProgressPosition(), 0.1f, 10f, 0.01f))
            {
                _isResetting = true;
                stateMachine.gameObject.GetComponent<MouseSpriteLogic>().GrabAnim();
                _progressBar.ResetProgress();
            }
        }
        else
        {
            MoveToward(_progressBar.GetProgressPosition(), 0.1f, 4f, 0.01f);
            if(_progressBar.CanStop())
                stateMachine.ChangeState(stateMachine.GetNewState());
        }
    }
    public override void Exit() 
    {
        stateMachine.gameObject.GetComponent<MouseSpriteLogic>().StopGrabAnim();
    }
}

