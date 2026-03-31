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
        BoxCollider2D box = window.BoxCollider;
        Vector2 size = box.size;
        Vector2 offset = box.offset;

        // Build box path
        PathD boxPath = new PathD();
        boxPath.Add(new PointD(offset.x - size.x / 2, offset.y - size.y / 2));
        boxPath.Add(new PointD(offset.x + size.x / 2, offset.y - size.y / 2));
        boxPath.Add(new PointD(offset.x + size.x / 2, offset.y + size.y / 2));
        boxPath.Add(new PointD(offset.x - size.x / 2, offset.y + size.y / 2));

        Vector2 localCircleCenter = box.transform.InverseTransformPoint(_circleCenter);
        Vector3 scale = box.transform.lossyScale;

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
        PathsD solution = Clipper.Difference(
            new PathsD { boxPath },
            new PathsD { circlePath },
            FillRule.NonZero
        );

        // Apply to PolygonCollider2D
        GameObject windowGO = box.gameObject;

        Destroy(box);
        PolygonCollider2D poly = windowGO.AddComponent<PolygonCollider2D>();
        poly.pathCount = solution.Count;

        for (int j = 0; j < solution.Count; j++)
        {
            Vector2[] points = new Vector2[solution[j].Count];
            for (int k = 0; k < solution[j].Count; k++)
            {
                points[k] = new Vector2((float)solution[j][k].x, (float)solution[j][k].y);
            }
            poly.SetPath(j, points);
        }
    }
}