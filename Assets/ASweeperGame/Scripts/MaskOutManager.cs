using UnityEngine;

public class MaskOutManager : MonoBehaviour
{
    private int _currentIndex = 3;

    public static MaskOutManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public int GetRenderingOrder()
    {
        _currentIndex += 2;
        return _currentIndex;
    }
}
