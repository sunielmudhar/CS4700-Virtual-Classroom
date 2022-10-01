using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineR;
    private int int_LineRef;
    List<Vector3> linePoints;

    //Generates a line reference used to uniquely identify each line that has been generated
    public void GenerateRef()
    {
        for (int i = 0; i < 6; i++)
        {
            int_LineRef = int_LineRef + Random.Range(1, 9);
        }
    }

    //Assign a line reference for lines that have been created by other participants
    public void SetLineRef(int lineRef)
    {
        int_LineRef = lineRef;
    }

    public int GetLineRef()
    {
        return int_LineRef;
    }

    //Update the line for as long as this function is recieving vector3 positions
    public void UpdateLine(Vector3 position)
    {
        if (linePoints == null)
        {
            linePoints = new List<Vector3>();
            SetLinePoint(position);
            return;
        }

        //Check if the same line, and the spacing between points is greater than 0.01f
        if (Vector3.Distance(linePoints.Last(), position) > 0.01f)
        {
            SetLinePoint(position);
        }
    }

    //Assign the line generated line points to the list
    void SetLinePoint(Vector3 point)
    {
        linePoints.Add(point);

        lineR.positionCount = linePoints.Count;
        lineR.SetPositions(linePoints.ToArray());
    }
}
