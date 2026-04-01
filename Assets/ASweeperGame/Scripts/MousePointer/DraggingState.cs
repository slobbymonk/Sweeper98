using System.Collections.Generic;
using UnityEngine;

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
        {
             stateMachine.ChangeState(stateMachine.GetNewState());
             return;
        }

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


