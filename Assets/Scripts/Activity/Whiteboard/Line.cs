using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineR;
    List<Vector3> linePoints;

    public void UpdateLine(Vector3 position)
    {
        
        if (linePoints == null)
        {
            linePoints = new List<Vector3>();
            SetLinePoint(position);
            return;
        }

        if (Vector3.Distance(linePoints.Last(), position) > 0.01f)
        {
            SetLinePoint(position);
        }
        
    }

    void SetLinePoint(Vector3 point)
    {
        linePoints.Add(point);

        lineR.positionCount = linePoints.Count;
        lineR.SetPositions(linePoints.ToArray());
    }
}
