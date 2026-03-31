using Clipper2Lib;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCutter : MonoBehaviour
{
    [SerializeField] private SpriteMask _mask;

    private void Awake()
    {
        _mask.enabled = false;
    }

    public void Cut()
    {
        _mask.enabled = true;
        transform.parent = null;
    }
}