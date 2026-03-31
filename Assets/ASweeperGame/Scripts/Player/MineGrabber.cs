using UnityEngine;

public class MineGrabber : MonoBehaviour
{
    private Mine _currentlyHeldMine;

    void LaunchBomb()
    {
        if (_currentlyHeldMine == null) return;

        // Launch logic

        _currentlyHeldMine = null;
    }

    public bool TryHoldMine(Mine mine)
    {
        if(mine != null) return false;
        _currentlyHeldMine = mine;
        return true;
    }
}
