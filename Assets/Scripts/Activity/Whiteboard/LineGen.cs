using Photon.Pun;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LineGen : MonoBehaviour
{
    [SerializeField] public GameObject linePrefab, participant, line;
    [SerializeField] public Line currentLine;
    [SerializeField] List<Line> currentLineList = new List<Line>();
    [SerializeField] static bool bl_CanDraw = false;
    PhotonView PV;

    Vector3 mousePosVec;
    RaycastHit hit;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bl_CanDraw)
        {
            participant = GameObject.Find("CurrentParticipant");

            if (Input.GetMouseButtonDown(0) && participant.tag.Equals("Student"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag.Equals("Plane"))   //Check to see if the raycast is hitting the plane
                {
                    line = Instantiate(linePrefab, hit.point, Quaternion.identity);
                    Line newLine = line.GetComponent<Line>();
                    newLine.GenerateRef();
                    currentLineList.Add(newLine);
                    currentLine = newLine;
                    PV.RPC("drawFunction", RpcTarget.Others, hit.point, newLine.GetLineRef());
                }
            }

            if (currentLine != null && Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    mousePosVec = hit.point;
                    PV.RPC("helperFunction", RpcTarget.All, "notNullActiveLine", mousePosVec, currentLine.GetLineRef());
                }

            }

            if (Input.GetMouseButtonUp(0))
            {
                PV.RPC("helperFunction", RpcTarget.All, "mouseUp", null, currentLine.GetLineRef());
            }
        }
    }

    [PunRPC]
    public void drawFunction(Vector3 hitPoint, int lineRef)
    {
        line = Instantiate(linePrefab, hitPoint, Quaternion.identity);
        Line newLine = line.GetComponent<Line>();
        newLine.SetLineRef(lineRef);
        currentLineList.Add(newLine);
    }

    [PunRPC]
    public void helperFunction(string action, Vector3 mousePosVec, int lineRef)
    {
        foreach (Line line in currentLineList)
        {
            if (line.GetLineRef() == lineRef)
            {
                Line activeLine = line;

                if (action.Equals("mouseUp"))
                {
                    activeLine = null;
                    currentLineList.Remove(line);
                }
                else if (action.Equals("notNullActiveLine"))
                {
                    activeLine.UpdateLine(mousePosVec);
                }
            }
        }
    }

    public static void CanDraw(bool state)
    {
        bl_CanDraw = state;
    }
}
