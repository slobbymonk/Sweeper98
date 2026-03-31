using UnityEngine;

public class MaskOutManager : MonoBehaviour
{
    private int _currentIndex = 5;

    public static MaskOutManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public int GetRenderingOrder()
    {
        return _currentIndex++;
    }
}
