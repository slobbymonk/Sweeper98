using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Spawns windows and holds a reference to them
/// </summary>
public class PopupWindowManager : MonoBehaviour
{
    [SerializeField] private float _popupTrySpawnTime = 5f;
    private float _timeSinceLastPopupSpawnAttempt = 0f;

    // Replace type GameObject -> Popup interface object
    [SerializeField] private List<PopupWindow> _popupWindows = new List<PopupWindow>();
    public List<PopupWindow> PopupWindows => _popupWindows;
    [SerializeField] private GameObject[] _popupPrefab;

    [SerializeField] private Transform[] _popupSpawnableArea;

    public static PopupWindowManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        
    }

    public void Update()
    {
        _timeSinceLastPopupSpawnAttempt += Time.deltaTime;
        if (_timeSinceLastPopupSpawnAttempt >= _popupTrySpawnTime)
        {
            _timeSinceLastPopupSpawnAttempt = 0f;
            TryToSpawnPopup();
        }
    }

    private void TryToSpawnPopup()
    {
        for (int i = 0; i < 5; i++)
        {
            Transform randomSpawnArea = _popupSpawnableArea[Random.Range(0, _popupSpawnableArea.Length)].transform;
            Vector2 randomPosition = new Vector2(
                Random.Range(randomSpawnArea.position.x - randomSpawnArea.localScale.x / 2, randomSpawnArea.position.x + randomSpawnArea.localScale.x / 2),
                Random.Range(randomSpawnArea.position.y - randomSpawnArea.localScale.y / 2, randomSpawnArea.position.y + randomSpawnArea.localScale.y / 2)
                );

            if (!IsOverlappingExistingPopup(randomPosition))
            {
                SpawnPopup(randomPosition);
                return;
            }
        }
    }
    private bool IsOverlappingExistingPopup(Vector2 popupArea)
    {
        foreach (var popup in _popupWindows)
        {
            if (Vector3.Distance(popup.BGSpriteRenderer.transform.position, popupArea) < popup.BGSpriteRenderer.transform.localScale.x
                || Vector3.Distance(popup.BGSpriteRenderer.transform.position, popupArea) < popup.BGSpriteRenderer.transform.localScale.y)
            {
                return true;
            }
        }
        return false;
    }
    void SpawnPopup(Vector2 position)
    {
        int randomPopupIndex = Random.Range(0, _popupPrefab.Length);
        PopupWindow popup = Instantiate(_popupPrefab[randomPopupIndex], position, Quaternion.identity).GetComponent<PopupWindow>();
        _popupWindows.Add(popup);
        popup.OnDestroyed += HandlePopupDestroyed;
    }

    private void HandlePopupDestroyed(PopupWindow window)
    {
        window.OnDestroyed -= HandlePopupDestroyed;
        _popupWindows.Remove(window);
    }
}
