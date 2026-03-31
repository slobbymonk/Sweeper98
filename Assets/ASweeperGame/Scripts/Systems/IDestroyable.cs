using System;

public interface IDestroyable
{
    Action<PopupWindow> OnDestroyed { get; set; }
}
