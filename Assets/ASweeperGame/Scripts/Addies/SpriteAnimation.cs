using System;
using UnityEngine;

[Serializable]
public struct Frame
{
    public float Duration;
    public Sprite Sprite;
}
[CreateAssetMenu(fileName = "FrameKeeperSO", menuName = "ScriptableObjects/AnimationFrames", order = 1)]
public class SpriteAnimation : ScriptableObject
{
    [SerializeField] private Frame[] _frames;
    public Frame[] Frames => _frames;
}