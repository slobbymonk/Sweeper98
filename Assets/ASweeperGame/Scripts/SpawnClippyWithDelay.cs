using PrimeTween;
using System;
using UnityEngine;

public class SpawnClippyWithDelay : MonoBehaviour
{
    [SerializeField] private StartingScreenManager _startingScreenManager;
    [SerializeField] private GameObject _clippy;

    private void Awake()
    {
        _startingScreenManager.OnWindowsPopUp += HandleSpawnClippy;
    }
    private void OnDestroy()
    {
        _startingScreenManager.OnWindowsPopUp -= HandleSpawnClippy;
    }

    private void HandleSpawnClippy()
    {
        _clippy.SetActive(true);
        Vector2 scale = _clippy.transform.localScale;
        _clippy.transform.localScale = scale * .5f;
        Tween.Scale(_clippy.transform, scale, .2f, Ease.OutBounce);
    }
}
