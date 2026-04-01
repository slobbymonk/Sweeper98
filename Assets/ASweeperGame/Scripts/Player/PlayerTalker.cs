using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerTalker : MonoBehaviour
{
    [SerializeField] private GameObject _bubble;
    [SerializeField] private TMP_Text _text;

    public static PlayerTalker Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowText(string text, float duration)
    {
        StartCoroutine(ShowTextRoutine(text, duration));
    }

    public IEnumerator ShowTextRoutine(string text, float duration)
    {
        _bubble.SetActive(true);
        _text.text = text;
        yield return new WaitForSeconds(duration);
        _bubble.SetActive(false);
    }
}