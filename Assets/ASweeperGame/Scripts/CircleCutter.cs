using Clipper2Lib;
using System.Collections.Generic;
using UnityEngine;

public class CircleCutter : MonoBehaviour
{
    [SerializeField] Vector2 _circleCenter;
    [SerializeField] int _circleResolution = 32;

    private void Update()
    {
        _circleCenter = transform.position;
    }

    public void Cut(PopupWindow window, float radius)
    {
        Collider2D collider = window.GetComponent<Collider2D>();
        if (collider == null) return;

        Transform colliderTransform = collider.transform;
        Vector2 localCircleCenter = colliderTransform.InverseTransformPoint(_circleCenter);
        Vector3 scale = colliderTransform.lossyScale;

        // Build subject path from whatever collider is present
        PathsD subject = new PathsD();

        if (collider is BoxCollider2D box)
        {
            Vector2 size = box.size;
            Vector2 offset = box.offset;

            PathD boxPath = new PathD
            {
                new PointD(offset.x - size.x / 2, offset.y - size.y / 2),
                new PointD(offset.x + size.x / 2, offset.y - size.y / 2),
                new PointD(offset.x + size.x / 2, offset.y + size.y / 2),
                new PointD(offset.x - size.x / 2, offset.y + size.y / 2)
            };
            subject.Add(boxPath);
        }
        else if (collider is PolygonCollider2D poly)
        {
            for (int i = 0; i < poly.pathCount; i++)
            {
                Vector2[] polyPoints = poly.GetPath(i);
                PathD path = new PathD();
                foreach (Vector2 p in polyPoints)
                    path.Add(new PointD(p.x, p.y));
                subject.Add(path);
            }
        }
        else
        {
            Debug.LogWarning($"CircleCutter: unsupported collider type {collider.GetType().Name}");
            return;
        }

        // Build circle path
        PathD circlePath = new PathD();
        for (int j = 0; j < _circleResolution; j++)
        {
            float angle = j * Mathf.PI * 2 / _circleResolution;
            circlePath.Add(new PointD(
                localCircleCenter.x + (radius / scale.x) * Mathf.Cos(angle),
                localCircleCenter.y + (radius / scale.y) * Mathf.Sin(angle)
            ));
        }

        // Subtract
        PathsD solution = Clipper.Difference(subject, new PathsD { circlePath }, FillRule.NonZero);

        // Replace collider with PolygonCollider2D
        GameObject windowGO = collider.gameObject;
        Destroy(collider);

        PolygonCollider2D result = windowGO.AddComponent<PolygonCollider2D>();
        result.pathCount = solution.Count;
        for (int j = 0; j < solution.Count; j++)
        {
            Vector2[] points = new Vector2[solution[j].Count];
            for (int k = 0; k < solution[j].Count; k++)
                points[k] = new Vector2((float)solution[j][k].x, (float)solution[j][k].y);

            result.SetPath(j, points);
        }
    }
}