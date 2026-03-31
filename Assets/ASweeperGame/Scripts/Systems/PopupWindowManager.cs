using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Spawns windows and holds a reference to them
/// </summary>
public class PopupWindowManager : MonoBehaviour
{
    // Replace type GameObject -> Popup interface object
    [SerializeField] private List<PopupWindow> _popupWindows = new List<PopupWindow>();
    public List<PopupWindow> PopupWindows => _popupWindows;
    [SerializeField] private GameObject[] _popupPrefab;

    public static PopupWindowManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SpawnPopup();
    }

    void SpawnPopup()
    {
        int randomPopupIndex = Random.Range(0, _popupWindows.Count);
        PopupWindow popup = Instantiate(_popupPrefab[randomPopupIndex], transform.position, Quaternion.identity).GetComponent<PopupWindow>();
        _popupWindows.Add(popup);
        popup.OnDestroyed += HandlePopupDestroyed;
    }

    private void HandlePopupDestroyed(PopupWindow window)
    {
        window.OnDestroyed -= HandlePopupDestroyed;
        _popupWindows.Remove(window);
    }
}
