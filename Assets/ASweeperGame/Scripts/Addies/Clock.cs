using System;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;

    private void Update()
    {
        _timeText.text = DateTime.Now.ToString();
    }
}
