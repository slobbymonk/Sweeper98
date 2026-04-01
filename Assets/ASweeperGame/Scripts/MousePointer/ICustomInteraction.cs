using UnityEngine;
using System;

public class ICustomInteraction : MonoBehaviour
{ 
    public struct InteractionPoint
    {
        public Vector2 position;
        public float speed;
        public float arrivalThreshold;
        public float waitAfter;
        public Action onReached;
    }
    public InteractionPoint[] GetInteractionPoints()
    {
        return new InteractionPoint[0];
    }
}
