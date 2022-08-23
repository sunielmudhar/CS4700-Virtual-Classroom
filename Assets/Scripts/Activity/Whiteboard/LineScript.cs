using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    public LineRenderer lineR;
    public List<Vector3> points;

    public void UpdateLine(Vector3 position)
    {
        if (points == null)
        {
            points = new List<Vector3>();
            SetPoint(position);
            return;
        }

        if (Vector3.Distance(points.Last(), position) > .1f)
        {
            SetPoint(position);
        }
    }

    public void SetPoint(Vector3 point)
    {
        points.Add(point);

        lineR.positionCount = points.Count;
        lineR.SetPositions(points.ToArray());
    }
}
