using System.Collections.Generic;
using Unity.VisualScripting;
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
            _skipToNext = true;
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
            Transform popupBackground = _targetPopup.transform.Find("BG").transform;
            Vector2 popupGrabPoint = (Vector2)popupBackground.position +
                new Vector2(0, popupBackground.localScale.y)+
                new Vector2(0, 0.1f);

            bool arrived = MoveToward(popupGrabPoint, 0.1f, 10f, 0.01f);
            if (arrived)
            {
                _holdsPopup = true;
                FindPopupDropzone();
                _targetPopup.transform.SetParent(stateMachine.Transform);
                _targetPopup.GrabLogic();
                stateMachine.gameObject.GetComponent<MouseSpriteLogic>().GrabAnim();
            }

        }
        else
        {
            if (MoveToward(_popupDropoffPoint, 0.1f, 2f, 0.5f))
            {
                stateMachine.ChangeState(stateMachine.GetNewState());
            }
        }
    }

    public override void Exit() 
    {
        stateMachine.gameObject.GetComponent<MouseSpriteLogic>().StopGrabAnim();

        if (_targetPopup == null)
            return;

        _targetPopup.HasBeenDragged = true;
        _targetPopup.transform.SetParent(null);
    }

    private void FindPopupDropzone()
    {
        _popupDropoffPoint = new Vector2(
            Random.Range(stateMachine.PopupDropzone.position.x - stateMachine.PopupDropzone.localScale.x / 2,
                         stateMachine.PopupDropzone.position.x + stateMachine.PopupDropzone.localScale.x / 2),
            Random.Range(stateMachine.PopupDropzone.position.y - stateMachine.PopupDropzone.localScale.y / 2,
                         stateMachine.PopupDropzone.position.y + stateMachine.PopupDropzone.localScale.y / 2));
    }


}


