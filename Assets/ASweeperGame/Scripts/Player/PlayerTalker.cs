using FMODUnity;
using PrimeTween;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerTalker : MonoBehaviour
{
    [SerializeField] private GameObject _bubble;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private EventReference _popupSound;
    [SerializeField] private string _textInAwake = "Oh oh, it looks like you've got a virus. Oh noooooo.";
    [SerializeField] private bool _playInAwake = true;

    private Vector3 _bubbleScale;

    public static PlayerTalker Instance;

    private void Awake()
    {
        Instance = this;

        _bubbleScale = _bubble.transform.localScale;

        if(_playInAwake) ShowText(_textInAwake, 5);
    }

    public void ShowText(string text, float duration)
    {
        RuntimeManager.PlayOneShot(_popupSound);
        StartCoroutine(ShowTextRoutine(text, duration));
    }

    public IEnumerator ShowTextRoutine(string text, float duration)
    {
        _bubble.SetActive(true);
        _text.text = text;

        _bubble.transform.localScale = Vector2.zero;
        Tween.Scale(_bubble.transform, _bubbleScale, .2f, Ease.OutBounce);

        yield return new WaitForSeconds(duration);

        Tween.Scale(_bubble.transform, Vector2.zero, .5f, Ease.InBounce);

        _bubble.SetActive(false);
    }
}