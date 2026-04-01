using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour, IDraggable
{
    public bool HasBeenDragged { get; set; }

    private float _timePlayed;

    [SerializeField] private Image _fillImage;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private float _timeBeforeFull;

    public Action<IDraggable> OnGrabbed { get; set; }

    void Update()
    {
        _timePlayed += Time.deltaTime;

        float progressPercentage = (_timePlayed / _timeBeforeFull) * 100;
        string progressPercentageString = progressPercentage.ToString("F0");
        _progressText.text = $"{progressPercentageString}%";
        _fillImage.fillAmount = progressPercentage / 100;
    }
    public void GrabLogic()
    {
        OnGrabbed?.Invoke(this);
    }
}
