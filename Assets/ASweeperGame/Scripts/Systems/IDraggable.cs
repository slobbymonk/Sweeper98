using System;

public interface IDraggable
{
    bool HasBeenDragged { get; set; }
    public Action<IDraggable> OnGrabbed { get; set; }
    void GrabLogic();
}
