using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour, IDraggable
{
    public bool HasBeenDragged { get; set; }

    private float _timePlayed;

    [SerializeField] private Image _fillImage;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private float _timeBeforeFull;

    [SerializeField] private float _boxWidth = 3.40515f;

    private bool _isFilling = true;

    private bool _hasBeenReset = false;
    public Action<IDraggable> OnGrabbed { get; set; }

    void Update()
    {

        if (_isFilling)
            _timePlayed += Time.deltaTime;
        else
        {
            _timePlayed -= Time.deltaTime * 5f;
            if(_timePlayed < 10)
            {
                _isFilling = true;
            }
        }

        float progressPercentage = (_timePlayed / _timeBeforeFull) * 100;

        if (progressPercentage > 80 && !_hasBeenReset)
            AskMouseForReset();




        string progressPercentageString = progressPercentage.ToString("F0");
        _progressText.text = $"{progressPercentageString}%";
        _fillImage.fillAmount = progressPercentage / 100;

    }
    public void GrabLogic()
    {
        OnGrabbed?.Invoke(this);
    }

    public void AskMouseForReset()
    {
        _hasBeenReset = true;
        FindAnyObjectByType<StateMachine>().StartResettingProgress(this);
    }

    public void ResetProgress()
    {
        _isFilling = false;
    }

    public Vector2 GetProgressPosition()
    {
        float progressPercentage = (_timePlayed / _timeBeforeFull);
        return _fillImage.transform.position + 
            new Vector3(-_boxWidth / 2, 0, 0) +
            new Vector3(_boxWidth * progressPercentage, 0, 0)+
            new Vector3(-0.2f, 0.2f);
    }

    public bool CanStop()
    {
        return _timePlayed < 10;
    }
}
