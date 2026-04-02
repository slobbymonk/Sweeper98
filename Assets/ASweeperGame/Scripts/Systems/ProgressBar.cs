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

    private bool _isFilling = true;
    public Action<IDraggable> OnGrabbed { get; set; }

    void Update()
    {

        if (_isFilling)
            _timePlayed += Time.deltaTime;
        else
            _timePlayed -= Time.deltaTime * 5f;


        float progressPercentage = (_timePlayed / _timeBeforeFull) * 100;

        string progressPercentageString = progressPercentage.ToString("F0");
        _progressText.text = $"{progressPercentageString}%";
        _fillImage.fillAmount = progressPercentage / 100;

    }
    public void GrabLogic()
    {
        OnGrabbed?.Invoke(this);
    }

    public bool CanReset()
    {
        float progressPercentage = (_timePlayed / _timeBeforeFull) * 100;
        return (progressPercentage < 0.6f);
    }

    public void ResetProgress()
    {

    }
}
