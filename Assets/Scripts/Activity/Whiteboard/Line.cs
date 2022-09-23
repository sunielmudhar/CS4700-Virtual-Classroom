using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineR;
    private int int_LineRef;
    List<Vector3> linePoints;

    public void GenerateRef()
    {
        for (int i = 0; i < 6; i++)
        {
            int_LineRef = int_LineRef + Random.Range(1, 9);
        }
    }

    public void SetLineRef(int lineRef)
    {
        int_LineRef = lineRef;
    }

    public int GetLineRef()
    {
        return int_LineRef;
    }

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
